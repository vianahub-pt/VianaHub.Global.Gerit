using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Data;

namespace VianaHub.Global.Gerit.Infra.Data.Interceptors;

public class TenantSessionCommandInterceptor : DbCommandInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<TenantSessionCommandInterceptor> _logger;

    public TenantSessionCommandInterceptor(IHttpContextAccessor httpContextAccessor, IHostEnvironment environment, ILogger<TenantSessionCommandInterceptor> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _environment = environment;
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        await EnsureSessionContextAsync(command, cancellationToken);
        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override async ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result,
        CancellationToken cancellationToken = default)
    {
        await EnsureSessionContextAsync(command, cancellationToken);
        return await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await EnsureSessionContextAsync(command, cancellationToken);
        return await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    private static bool ParseBoolHeader(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (bool.TryParse(value, out var b))
            return b;

        // Accept numeric truthy values like "1" or "0"
        if (int.TryParse(value, out var i))
            return i != 0;

        // Accept common truthy strings
        var v = value.Trim().ToLowerInvariant();
        return v == "yes" || v == "y" || v == "on";
    }

    private async Task EnsureSessionContextAsync(DbCommand command, CancellationToken cancellationToken)
    {
        // Avoid recursion: if the current command already sets session context, skip
        if (command.CommandText != null && command.CommandText.IndexOf("sp_set_session_context", StringComparison.OrdinalIgnoreCase) >= 0)
            return;

        if (command.Connection is not SqlConnection sqlConnection)
            return;

        var httpContext = _httpContextAccessor.HttpContext;

        // If no HttpContext and not Development, skip (safety)
        if (httpContext == null && !_environment.IsDevelopment())
        {
            _logger.LogDebug("[RLS] No HttpContext and not Development. Skipping session context setup.");
            return;
        }

        // Determine tenant and superadmin similar to connection interceptor
        int tenantValue = 0;
        var isSuperAdmin = false;

        if (httpContext != null)
        {
            var user = httpContext.User;
            if (user?.Identity is { IsAuthenticated: true })
            {
                var tenantIdClaim = user.FindFirst("tenant_id") ?? user.FindFirst("tenant") ?? user.FindFirst("tenantId");
                var isSuperAdminClaim = user.FindFirst("is_super_admin") ?? user.FindFirst("isSuperAdmin") ?? user.FindFirst("superadmin");

                if (tenantIdClaim is not null && int.TryParse(tenantIdClaim.Value, out var t))
                    tenantValue = t;

                if (isSuperAdminClaim is not null && bool.TryParse(isSuperAdminClaim.Value, out var s))
                    isSuperAdmin = s;
            }
        }

        // Development fallback: prefer headers if provided, otherwise force superadmin when unauthenticated
        if (_environment.IsDevelopment())
        {
            var headers = httpContext?.Request?.Headers;
            if (headers != null && (headers.ContainsKey("x-tenant-id") || headers.ContainsKey("x-super-admin")))
            {
                int tenantFromHeader = 0;
                bool superAdminFromHeader = false;

                if (headers.TryGetValue("x-tenant-id", out var headerTenant))
                    int.TryParse(headerTenant.ToString(), out tenantFromHeader);

                if (headers.TryGetValue("x-super-admin", out var headerSuper))
                    superAdminFromHeader = ParseBoolHeader(headerSuper.ToString());

                tenantValue = tenantFromHeader;
                isSuperAdmin = superAdminFromHeader;

                _logger.LogDebug("[RLS] Development header fallback applied. TenantId={TenantId}, IsSuperAdmin={IsSuperAdmin}", tenantValue, isSuperAdmin);
            }
            else if (httpContext == null || httpContext.User?.Identity is not { IsAuthenticated: true })
            {
                isSuperAdmin = true;
                tenantValue = 0;
                _logger.LogDebug("[RLS] Development debug path: forcing SuperAdmin session context.");
            }
        }

        try
        {
            await using var cmd = sqlConnection.CreateCommand();
            // Sempre definir o SESSION_CONTEXT para garantir que conexões reusadas do pool sejam atualizadas
            cmd.CommandText = @"EXEC sp_set_session_context @key=N'IsSuperAdmin', @value=@isSuperAdmin;
EXEC sp_set_session_context @key=N'TenantId', @value=@tenantId;";

            var pSuper = cmd.CreateParameter();
            pSuper.ParameterName = "@isSuperAdmin";
            pSuper.Value = isSuperAdmin ? 1 : 0;
            cmd.Parameters.Add(pSuper);

            var pTenant = cmd.CreateParameter();
            pTenant.ParameterName = "@tenantId";
            pTenant.Value = tenantValue;
            cmd.Parameters.Add(pTenant);

            await cmd.ExecuteNonQueryAsync(cancellationToken);

            _logger.LogDebug("[RLS] Ensured session context. TenantId={TenantId}, IsSuperAdmin={IsSuperAdmin}", tenantValue, isSuperAdmin);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[RLS] Failed to ensure session context before command execution");
        }
    }
}
