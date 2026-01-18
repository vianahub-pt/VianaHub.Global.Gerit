using Microsoft.Extensions.DependencyInjection;
using VianaHub.Global.Gerit.Application.Interfaces;
using VianaHub.Global.Gerit.Application.Services;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Validators.Action;
using VianaHub.Global.Gerit.Domain.Validators.Plan;
using VianaHub.Global.Gerit.Domain.Validators.Resource;
using VianaHub.Global.Gerit.Domain.Validators.Role;
using VianaHub.Global.Gerit.Domain.Validators.Subscription;
using VianaHub.Global.Gerit.Domain.Validators.Tenant;
using VianaHub.Global.Gerit.Domain.Validators.User;
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

        // Validators (Scoped)
        services.AddScoped<IEntityDomainValidator<ActionEntity>, ActionValidator>();
        services.AddScoped<IEntityDomainValidator<ResourceEntity>, ResourceValidator>();
        services.AddScoped<IEntityDomainValidator<RoleEntity>, RoleValidator>();
        services.AddScoped<IEntityDomainValidator<PlanEntity>, PlanValidator>();
        services.AddScoped<IEntityDomainValidator<TenantEntity>, TenantValidator>();
        services.AddScoped<IEntityDomainValidator<SubscriptionEntity>, SubscriptionValidator>();
        services.AddScoped<IEntityDomainValidator<UserEntity>, UserValidator>();

        // Application
        services.AddScoped<IActionAppService, ActionAppService>();
        services.AddScoped<IResourceAppService, ResourceAppService>();
        services.AddScoped<IRoleAppService, RoleAppService>();
        services.AddScoped<IPlanAppService, PlanAppService>();
        services.AddScoped<ITenantAppService, TenantAppService>();
        services.AddScoped<ISubscriptionAppService, SubscriptionAppService>();
        services.AddScoped<IUserAppService, UserAppService>();

        // Domain
        services.AddScoped<IActionDomainService, ActionDomainService>();
        services.AddScoped<IResourceDomainService, ResourceDomainService>();
        services.AddScoped<IRoleDomainService, RoleDomainService>();
        services.AddScoped<IPlanDomainService, PlanDomainService>();
        services.AddScoped<ITenantDomainService, TenantDomainService>();
        services.AddScoped<ISubscriptionDomainService, SubscriptionDomainService>();
        services.AddScoped<IUserDomainService, UserDomainService>();

        // Infra.Data - Repositories
        services.AddScoped<IActionDataRepository, ActionDataRepository>();
        services.AddScoped<IResourceDataRepository, ResourceDataRepository>();
        services.AddScoped<IRoleDataRepository, RoleDataRepository>();
        services.AddScoped<IPlanDataRepository, PlanDataRepository>();
        services.AddScoped<ITenantDataRepository, TenantDataRepository>();
        services.AddScoped<ISubscriptionDataRepository, SubscriptionDataRepository>();
        services.AddScoped<IJwtKeyDataRepository, JwtKeyDataRepository>();
        services.AddScoped<IUserDataRepository, UserDataRepository>();

        return services;
    }
}

