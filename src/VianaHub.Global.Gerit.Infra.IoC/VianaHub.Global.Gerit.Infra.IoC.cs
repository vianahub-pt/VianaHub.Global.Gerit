using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Application.Services;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Infra.Data.Repository;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Infra.IoC;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeritInfrastructureExtensions(this IServiceCollection services)
    {
        // ... existing registrations ...

        // JwtKeys
        services.AddScoped<IJwtKeyDataRepository, JwtKeyDataRepository>();
        services.AddScoped<IJwtKeyAppService, JwtKeyAppService>();
        services.AddScoped<IJwtKeyDomainService, JwtKeyDomainService>();

        // Notifications
        services.AddScoped<INotify, Notify>();

        return services;
    }
}
