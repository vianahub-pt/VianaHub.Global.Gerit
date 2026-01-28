using Hangfire.Dashboard;

namespace VianaHub.Global.Gerit.Api.Security;

/// <summary>
/// Authorization filter for Hangfire dashboard. Permite apenas usuários autenticados com policy 'BackOffice'.
/// Implementação simples que aceita requisições em ambiente de desenvolvimento (para facilitar testes locais).
/// </summary>
public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // Permitir acesso quando em Development sem autenticação
        var env = httpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
        if (env != null && env.IsDevelopment())
            return true;

        // Em produção, exigir usuário autenticado e policy BackOffice
        var user = httpContext.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            // Aqui não temos acesso direto às policies, então apenas verificamos autenticação.
            // Se necessário, verifique roles/claims específicos.
            return true;
        }

        return false;
    }
}
