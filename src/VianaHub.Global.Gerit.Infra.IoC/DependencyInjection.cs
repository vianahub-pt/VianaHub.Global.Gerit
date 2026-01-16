using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Application.Services;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Infra.Data.Repository;

namespace VianaHub.Global.Gerit.Infra.IoC;

/// <summary>
/// Configuração centralizada de injeção de dependências para todas as camadas do Gerit.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra todos os serviços da aplicação no container de DI.
    /// </summary>
    public static IServiceCollection AddGeritInfrastructure(this IServiceCollection services)
    {
        // Notificações (Scoped para manter estado durante a requisição)
        services.AddScoped<INotify, Notify>();

        // Application
        services.AddScoped<IActionAppService, ActionAppService>();

        // Domain
        services.AddScoped<IActionDomainService, ActionDomainService>();

        // Infra.Data - Repositories
        services.AddScoped<IActionDataRepository, ActionDataRepository>();
        services.AddScoped<IJwtKeyDataRepository, JwtKeyDataRepository>();

        return services;
    }
}

