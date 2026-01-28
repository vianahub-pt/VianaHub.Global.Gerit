using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
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
using VianaHub.Global.Gerit.Domain.Validators.UserRole;
using VianaHub.Global.Gerit.Domain.Validators.RolePermission;
using VianaHub.Global.Gerit.Domain.Validators.Job;
using VianaHub.Global.Gerit.Infra.Data.Repository;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Infra.Messaging;
using VianaHub.Global.Gerit.Infra.Data.Context;
using AutoMapper;
using System.Reflection;
using VianaHub.Global.Gerit.Infra.Job.Services;
using VianaHub.Global.Gerit.Infra.Job.HostedServices;
using VianaHub.Global.Gerit.Domain.Interfaces.Job;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;
using VianaHub.Global.Gerit.Domain.Validators.Jwt;
using VianaHub.Global.Gerit.Application.Services.Identity;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Services.Identity;
using VianaHub.Global.Gerit.Infra.Data.Repository.Identity;

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
        services.AddScoped<IValidator<UserRoleEntity>, UserRoleValidator>();
        services.AddScoped<IValidator<RolePermissionEntity>, RolePermissionValidator>();

        // Register JobDefinition validator
        services.AddScoped<IEntityDomainValidator<JobDefinitionEntity>, JobDefinitionValidator>();

        // Register JwtKey validator
        services.AddScoped<IEntityDomainValidator<JwtKeyEntity>, JwtKeyValidator>();

        // Application
        services.AddScoped<IActionAppService, ActionAppService>();
        services.AddScoped<IResourceAppService, ResourceAppService>();
        services.AddScoped<IRoleAppService, RoleAppService>();
        services.AddScoped<IPlanAppService, PlanAppService>();
        services.AddScoped<ITenantAppService, TenantAppService>();
        services.AddScoped<ISubscriptionAppService, SubscriptionAppService>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IUserRoleAppService, UserRoleAppService>();
        services.AddScoped<IUserRoleDomainService, UserRoleDomainService>();
        services.AddScoped<IRolePermissionAppService, RolePermissionAppService>();
        services.AddScoped<IAuthAppService, AuthAppService>();
        services.AddScoped<IJobAppService, JobAppService>();
        services.AddScoped<IJwtKeyAppService, JwtKeyAppService>();

        // Domain
        services.AddScoped<IActionDomainService, ActionDomainService>();
        services.AddScoped<IResourceDomainService, ResourceDomainService>();
        services.AddScoped<IRoleDomainService, RoleDomainService>();
        services.AddScoped<IPlanDomainService, PlanDomainService>();
        services.AddScoped<ITenantDomainService, TenantDomainService>();
        services.AddScoped<ISubscriptionDomainService, SubscriptionDomainService>();
        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IRolePermissionDomainService, RolePermissionDomainService>();
        services.AddScoped<IJwtKeyDomainService, JwtKeyDomainService>();
        // Infra.Data - Repositories
        services.AddScoped<IActionDataRepository, ActionDataRepository>();
        services.AddScoped<IResourceDataRepository, ResourceDataRepository>();
        services.AddScoped<IRoleDataRepository, RoleDataRepository>();
        services.AddScoped<IPlanDataRepository, PlanDataRepository>();
        services.AddScoped<ITenantDataRepository, TenantDataRepository>();
        services.AddScoped<ISubscriptionDataRepository, SubscriptionDataRepository>();
        services.AddScoped<IJwtKeyDataRepository, JwtKeyDataRepository>();
        services.AddScoped<IUserDataRepository, UserDataRepository>();
        services.AddScoped<IUserRoleDataRepository, UserRoleDataRepository>();
        services.AddScoped<IRolePermissionDataRepository, RolePermissionDataRepository>();
        services.AddScoped<IRefreshTokenDataRepository, RefreshTokenDataRepository>();
        services.AddScoped<IJobDefinitionDataRepository, JobDefinitionDataRepository>();

        // Infra.Messaging (Email sender no-op por enquanto)
        services.AddScoped<IEmailSender, NoOpEmailSender>();

        // Hangfire Job service
        services.AddScoped<IJobSchedulerService, HangfireJobService>();
        services.AddScoped<IJobExecutor, HangfireJobExecutor>();
        services.AddScoped<IJobSyncService, VianaHub.Global.Gerit.Infra.Job.Services.JobSyncService>();
        // Register the new scheduled sync job so it can be resolved by the job scheduler
        services.AddScoped<VianaHub.Global.Gerit.Infra.Job.Jobs.Maintenance.ScheduledSyncJobDefinitionsJob>();
        // Register JWT rotation job
        services.AddScoped<VianaHub.Global.Gerit.Infra.Job.Jobs.Security.JwtKeyRotationJob>();
        services.AddHostedService<JobSyncHostedService>();

        // Data Context
        services.AddScoped<GeritDbContext>();

        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Secret provider (chave mestra gerenciada externamente) - por padrão lê variável de ambiente JWT_MASTER_KEY
        services.AddSingleton<VianaHub.Global.Gerit.Domain.Interfaces.ISecretProvider, SecretProviderEnvironment>();

        return services;
    }
}

