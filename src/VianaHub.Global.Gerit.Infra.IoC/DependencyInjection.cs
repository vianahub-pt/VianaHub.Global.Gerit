using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Infra.IoC;

/// <summary>
/// Configuração centralizada de injeção de dependências para todas as camadas do Gerit.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos os serviços da aplicação no container de DI.
    /// </summary>
    public static IServiceCollection AddGeritInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Notificações (Scoped para manter estado durante a requisição)
        services.AddScoped<INotify, Notify>();

        return services;
    }

    
}

