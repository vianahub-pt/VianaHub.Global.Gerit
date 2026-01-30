using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Application.Services.Identity;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Services.Identity;
using VianaHub.Global.Gerit.Infra.Data.Repository.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;

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
