using Hangfire.Dashboard;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace VianaHub.Global.Gerit.Api.Security;

/// <summary>
/// Configuração do dashboard do Hangfire.
/// </summary>
public class HangfireDashboardSettings
{
    /// <summary>
    /// Habilita o dashboard do Hangfire.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Habilita autenticação Basic Auth no dashboard.
    /// Quando false e o ambiente não é Development, o acesso é bloqueado.
    /// </summary>
    public bool RequireBasicAuth { get; set; } = true;

    /// <summary>
    /// Utilizador para Basic Auth.
    /// </summary>
    public string Username { get; set; } = "hangfire";

    /// <summary>
    /// Password para Basic Auth.
    /// </summary>
    public string Password { get; set; } = "changeme";
}

/// <summary>
/// Authorization filter para o Hangfire Dashboard.
/// 
/// Comportamento por ambiente:
/// - Development (com debugger): acesso livre para facilitar testes locais.
/// - Outros ambientes: requer Basic Auth configurada via appsettings (secção "HangfireDashboard").
/// 
/// O uso de Basic Auth é necessário porque o browser nunca envia o header
/// "Authorization: Bearer ..." em navegação normal — tornando a validação
/// de JWT inútil para proteger o dashboard.
/// </summary>
public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly HangfireDashboardSettings _settings;
    private readonly ILogger<HangfireDashboardAuthorizationFilter> _logger;

    public HangfireDashboardAuthorizationFilter(
        IOptions<HangfireDashboardSettings> settings,
        ILogger<HangfireDashboardAuthorizationFilter> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var env = httpContext.RequestServices.GetService<IWebHostEnvironment>();
        var isDebugDevelopment = env != null && env.IsDevelopment() && System.Diagnostics.Debugger.IsAttached;

        // Em Development com debugger anexado, libera sem autenticação (apenas local)
        if (isDebugDevelopment)
        {
            _logger.LogDebug("[Hangfire] Acesso ao dashboard liberado: ambiente Development com debugger.");
            return true;
        }

        // Se Basic Auth não está configurado, bloqueia por segurança
        if (!_settings.RequireBasicAuth)
        {
            _logger.LogWarning("[Hangfire] Acesso ao dashboard bloqueado: RequireBasicAuth=false fora de Development. Configure HangfireDashboard:RequireBasicAuth=true.");
            return false;
        }

        // Validar credenciais Basic Auth enviadas pelo browser
        var authHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            // Sem credenciais: emitir desafio WWW-Authenticate para o browser abrir o popup de login
            ChallengeBasicAuth(httpContext);
            return false;
        }

        try
        {
            var encodedCredentials = authHeader["Basic ".Length..].Trim();
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var separatorIndex = decoded.IndexOf(':');
            if (separatorIndex < 0)
            {
                ChallengeBasicAuth(httpContext);
                return false;
            }

            var username = decoded[..separatorIndex];
            var password = decoded[(separatorIndex + 1)..];

            var valid = string.Equals(username, _settings.Username, StringComparison.Ordinal)
                     && string.Equals(password, _settings.Password, StringComparison.Ordinal);

            if (!valid)
            {
                _logger.LogWarning("[Hangfire] Tentativa de acesso ao dashboard com credenciais inválidas. Username: {Username} | IP: {IP}",
                    username,
                    httpContext.Connection.RemoteIpAddress);
                ChallengeBasicAuth(httpContext);
                return false;
            }

            _logger.LogInformation("[Hangfire] Acesso ao dashboard autorizado. Username: {Username} | IP: {IP}",
                username,
                httpContext.Connection.RemoteIpAddress);
            return true;
        }
        catch (FormatException)
        {
            ChallengeBasicAuth(httpContext);
            return false;
        }
    }

    private static void ChallengeBasicAuth(HttpContext httpContext)
    {
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\", charset=\"UTF-8\"";
    }
}
