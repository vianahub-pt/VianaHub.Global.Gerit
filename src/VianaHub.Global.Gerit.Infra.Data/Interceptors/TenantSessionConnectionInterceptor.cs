using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Diagnostics;

namespace VianaHub.Global.Gerit.Infra.Data.Interceptors;

// Interceptor responsável por popular SESSION_CONTEXT com TenantId e IsSuperAdmin
// baseado exclusivamente nos claims do JWT do usuário autenticado.
// Em Development, quando não há token JWT, automaticamente habilita IsSuperAdmin para facilitar testes.
// Em Jobs (identificados via IJobContext), sempre habilita IsSuperAdmin para permitir acesso cross-tenant.
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
            _logger.LogInformation("[RLS] No HttpContext. Assuming background job. Enabling SuperAdmin session context.");

            await SetDevelopmentSuperAdminContextAsync(connection, cancellationToken);
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

        // If running in Development and debugger is attached, always enable SuperAdmin to simplify local debugging
        if (_environment.IsDevelopment() && Debugger.IsAttached)
        {
            _logger.LogInformation("?? [RLS] Debugger attached in Development. Forcing SuperAdmin session context for local debugging.");
            await SetDevelopmentSuperAdminContextAsync(connection, cancellationToken);
            return;
        }

        // Em Development, se não há usuário authenticated, habilita IsSuperAdmin automaticamente
        //if (_environment.IsDevelopment() && (user?.Identity is not { IsAuthenticated: true }))
        //{
        //    // Antes de setar o SuperAdmin automático, verificar se há cabeçalhos de fallback (apenas em Development)
        //    var headers = httpContext.Request.Headers;
        //    if (headers != null && (headers.ContainsKey("x-tenant-id") || headers.ContainsKey("x-super-admin")))
        //    {
        //        // Use header values se presentes
        //        int tenantFromHeader = 0;
        //        bool superAdminFromHeader = false;

        //        if (headers.TryGetValue("x-tenant-id", out var headerTenant))
        //            int.TryParse(headerTenant.ToString(), out tenantFromHeader);

        //        if (headers.TryGetValue("x-super-admin", out var headerSuper))
        //            bool.TryParse(headerSuper.ToString(), out superAdminFromHeader);

        //        _logger.LogInformation("?? [RLS] Development mode with header fallback. Using x-tenant-id={Tenant}, x-super-admin={Super}", tenantFromHeader, superAdminFromHeader);

        //        await SetSessionContextFromValuesAsync(connection, tenantFromHeader, superAdminFromHeader, cancellationToken);
        //        return;
        //    }

        //    _logger.LogInformation("?? [RLS] Development mode without authentication. Setting SuperAdmin context for local debugging.");
        //    await SetDevelopmentSuperAdminContextAsync(connection, cancellationToken);
        //    return;
        //}

        if (user?.Identity is not { IsAuthenticated: true })
        {
            var headers = httpContext.Request.Headers;
            if (headers.TryGetValue("x-tenant-id", out var headerTenant))
            {
                int.TryParse(headerTenant.ToString(), out var tenantFromHeader);
                await SetSessionContextFromValuesAsync(connection, tenantFromHeader, false, cancellationToken);
                return;
            }
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
                // Em Development, antes de setar automático, verificar cabeçalhos
                var headers = httpContext.Request.Headers;
                if (headers != null && (headers.ContainsKey("x-tenant-id") || headers.ContainsKey("x-super-admin")))
                {
                    int tenantFromHeader = 0;
                    bool superAdminFromHeader = false;

                    if (headers.TryGetValue("x-tenant-id", out var headerTenant))
                        int.TryParse(headerTenant.ToString(), out tenantFromHeader);

                    if (headers.TryGetValue("x-super-admin", out var headerSuper))
                        bool.TryParse(headerSuper.ToString(), out superAdminFromHeader);

                    _logger.LogInformation("?? [RLS] Development mode without tenant claims but header provided. Using x-tenant-id={Tenant}, x-super-admin={Super}", tenantFromHeader, superAdminFromHeader);
                    await SetSessionContextFromValuesAsync(connection, tenantFromHeader, superAdminFromHeader, cancellationToken);
                    return;
                }

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
        int tenantValue = 0; // Default para SuperAdmin
        
        if (tenantIdClaim is not null && int.TryParse(tenantIdClaim.Value, out var tenantInt))
        {
            tenantValue = tenantInt;
            _logger.LogDebug("?? [RLS] Setting TenantId context: {TenantId}", tenantInt);
        }
        else if (tenantIdClaim is not null)
        {
            _logger.LogWarning("?? [RLS] TenantId claim value is not a valid INT: {TenantId}", tenantIdClaim.Value);
            tenantValue = 0; // Fallback para SuperAdmin mode
        }
        else
        {
            // Super admin sem tenant -> usa 0
            _logger.LogDebug("?? [RLS] SuperAdmin without tenant. Using 0 as TenantId.");
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
        tenantParam.Value = 0; // 0 para super admin (INT ao invés de GUID)
        cmd.Parameters.Add(tenantParam);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation(
            "? [RLS] Development SuperAdmin context configured successfully. IsSuperAdmin=1, TenantId={TenantId}",
            0);
    }

    private async Task SetSessionContextFromValuesAsync(DbConnection connection, int tenantId, bool isSuperAdmin, CancellationToken cancellationToken)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=@isSuperAdmin; EXEC sp_set_session_context @key=N'TenantId', @value=@tenantId;";

        var pSuper = cmd.CreateParameter();
        pSuper.ParameterName = "@isSuperAdmin";
        pSuper.Value = isSuperAdmin ? 1 : 0;
        cmd.Parameters.Add(pSuper);

        var pTenant = cmd.CreateParameter();
        pTenant.ParameterName = "@tenantId";
        pTenant.Value = tenantId;
        cmd.Parameters.Add(pTenant);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation("?? [RLS] Session context set from header values. TenantId={TenantId}, IsSuperAdmin={IsSuperAdmin}", tenantId, isSuperAdmin);
    }

    /// <summary>
    /// Configura o SESSION_CONTEXT para modo Super Admin em jobs de background.
    /// Jobs em background precisam acessar dados de múltiplos tenants (ex: ReconcileJwtKeys, CleanupExpiredSessions).
    /// </summary>
    private async Task SetJobSuperAdminContextAsync(
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
        tenantParam.Value = 0; // 0 para super admin (INT ao invés de GUID)
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
