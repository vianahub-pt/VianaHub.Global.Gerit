using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace VianaHub.Global.Gerit.Infra.Data.Interceptors;

// Interceptor responsável por popular SESSION_CONTEXT com TenantId e IsSuperAdmin
// baseado exclusivamente nos claims do JWT do usuário autenticado.
// Em Development, quando não há token JWT, automaticamente habilita IsSuperAdmin para facilitar testes.
// Em Jobs em Background (identificados via IBackgroundJobContext), sempre habilita IsSuperAdmin para permitir acesso cross-tenant.
public class TenantSessionConnectionInterceptor : DbConnectionInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _environment;

    private readonly ILogger<TenantSessionConnectionInterceptor> _logger;

    public TenantSessionConnectionInterceptor(
        IHttpContextAccessor httpContextAccessor,
        IHostEnvironment environment,
        ILogger<TenantSessionConnectionInterceptor> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
        _logger = logger;
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);

        // Ignora conexões que não sejam SQL Server
        if (connection is not SqlConnection)
        {
            return;
        }

        // ??? SEGURANÇA: Se não há HttpContext E não é um job em background, algo está errado
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning(
                "?? [RLS] Connection opened without HttpContext and outside background job context. " +
                "This may indicate a migration, test, or console app. RLS will NOT be bypassed for security.");

            // NÃO configura SuperAdmin - deixa o RLS bloquear por segurança
            // Se for uma migração/seed legítima, deve configurar o contexto manualmente
            return;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        var user = httpContext.User;

        // ?? LOG DETALHADO PARA DEBUG
        _logger.LogDebug(
            "?? [RLS] Connection opened. Environment: {Environment}, IsAuthenticated: {IsAuthenticated}, HasUser: {HasUser}",
            _environment.EnvironmentName,
            user?.Identity?.IsAuthenticated ?? false,
            user != null);

        // Em Development, se não há usuário autenticado, habilita IsSuperAdmin automaticamente
        if (_environment.IsDevelopment() && (user?.Identity is not { IsAuthenticated: true }))
        {
            _logger.LogInformation("?? [RLS] Development mode without authentication. Setting SuperAdmin context for local debugging.");
            await SetDevelopmentSuperAdminContextAsync(connection, cancellationToken);
            return;
        }

        if (user?.Identity is not { IsAuthenticated: true })
        {
            _logger.LogDebug("?? [RLS] Unauthenticated user. RLS will enforce tenant isolation.");
            return; // Usuário não autenticado -> não define TenantId. RLS mantém isolamento.
        }

        var tenantIdClaim = user.FindFirst("tenant_id") ?? user.FindFirst("tenant") ?? user.FindFirst("tenantId");
        var isSuperAdminClaim = user.FindFirst("is_super_admin") ?? user.FindFirst("isSuperAdmin") ?? user.FindFirst("superadmin");

        _logger.LogDebug(
            "?? [RLS] Claims found. TenantId: {HasTenant}, IsSuperAdmin: {HasSuperAdmin}",
            tenantIdClaim != null,
            isSuperAdminClaim != null);

        if (tenantIdClaim is null && isSuperAdminClaim is null)
        {
            // Nenhum tenant e nenhum super admin -> em Development, habilita super admin
            if (_environment.IsDevelopment())
            {
                _logger.LogInformation("?? [RLS] Development mode without tenant claims. Setting SuperAdmin context for local debugging.");
                await SetDevelopmentSuperAdminContextAsync(connection, cancellationToken);
                return;
            }

            _logger.LogDebug("?? [RLS] No tenant or superadmin claims. RLS will enforce tenant isolation.");
            // Em Production, não define nada
            return;
        }

        await using var cmd = connection.CreateCommand();

        // Comando base para TenantId (sempre existe parâmetro @tenantId)
        cmd.CommandText = @"EXEC sp_set_session_context @key=N'TenantId', @value=@tenantId;";

        // Super admin: habilita flag que sua POLICY usa para ignorar TenantId
        if (isSuperAdminClaim is not null && bool.TryParse(isSuperAdminClaim.Value, out var isSuperAdmin) && isSuperAdmin)
        {
            cmd.CommandText += @"EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;";
            _logger.LogInformation("?? [RLS] SuperAdmin user authenticated. RLS bypass enabled.");
        }

        // Fonte da verdade: claim do JWT, nunca header
        object tenantValue;
        if (tenantIdClaim is not null && Guid.TryParse(tenantIdClaim.Value, out var tenantGuid))
        {
            tenantValue = tenantGuid;
            _logger.LogDebug("?? [RLS] Setting TenantId context: {TenantId}", tenantGuid);
        }
        else if (tenantIdClaim is not null)
        {
            tenantValue = tenantIdClaim.Value; // fallback string
            _logger.LogDebug("?? [RLS] Setting TenantId context (string): {TenantId}", tenantIdClaim.Value);
        }
        else
        {
            // Super admin sem tenant -> usa Guid.Empty apenas para satisfazer o parâmetro
            tenantValue = Guid.Empty;
            _logger.LogDebug("?? [RLS] SuperAdmin without tenant. Using Guid.Empty.");
        }

        var tenantParam = cmd.CreateParameter();
        tenantParam.ParameterName = "@tenantId";
        tenantParam.Value = tenantValue;
        cmd.Parameters.Add(tenantParam);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Configura o SESSION_CONTEXT para modo Super Admin em ambiente de desenvolvimento.
    /// Permite acesso irrestrito aos dados para facilitar testes locais.
    /// </summary>
    private async Task SetDevelopmentSuperAdminContextAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "?? [RLS] Configuring Development SuperAdmin context. Database: {Database}",
            connection.Database);

        await using var cmd = connection.CreateCommand();

        // Define IsSuperAdmin = 1 para bypass do RLS
        cmd.CommandText = @"
            EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
            EXEC sp_set_session_context @key=N'TenantId', @value=@tenantId;";

        var tenantParam = cmd.CreateParameter();
        tenantParam.ParameterName = "@tenantId";
        tenantParam.Value = Guid.Empty; // Guid.Empty para super admin
        cmd.Parameters.Add(tenantParam);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation(
            "? [RLS] Development SuperAdmin context configured successfully. IsSuperAdmin=1, TenantId={TenantId}",
            Guid.Empty);
    }

    /// <summary>
    /// Configura o SESSION_CONTEXT para modo Super Admin em jobs de background.
    /// Jobs em background precisam acessar dados de múltiplos tenants (ex: ReconcileJwtKeys, CleanupExpiredSessions).
    /// </summary>
    private async Task SetBackgroundJobSuperAdminContextAsync(
        DbConnection connection,
        string? jobName,
        string? executionId,
        CancellationToken cancellationToken)
    {
        await using var cmd = connection.CreateCommand();

        // Define IsSuperAdmin = 1 para bypass do RLS em jobs de background
        cmd.CommandText = @"
            EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=1;
            EXEC sp_set_session_context @key=N'TenantId', @value=@tenantId;
            EXEC sp_set_session_context @key=N'JobName', @value=@jobName;
            EXEC sp_set_session_context @key=N'JobExecutionId', @value=@executionId;";

        var tenantParam = cmd.CreateParameter();
        tenantParam.ParameterName = "@tenantId";
        tenantParam.Value = Guid.Empty; // Guid.Empty para super admin
        cmd.Parameters.Add(tenantParam);

        var jobNameParam = cmd.CreateParameter();
        jobNameParam.ParameterName = "@jobName";
        jobNameParam.Value = (object?)jobName ?? DBNull.Value;
        cmd.Parameters.Add(jobNameParam);

        var executionIdParam = cmd.CreateParameter();
        executionIdParam.ParameterName = "@executionId";
        executionIdParam.Value = (object?)executionId ?? DBNull.Value;
        cmd.Parameters.Add(executionIdParam);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation(
            "? [RLS] Background job SESSION_CONTEXT configured. Job: {JobName}, ExecutionId: {ExecutionId}",
            jobName ?? "Unknown",
            executionId ?? "Unknown");
    }
}
