using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Validators.Job;
using VianaHub.Global.Gerit.Infra.Integration.Messaging;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Infra.Data.Security;
using System.Reflection;
using VianaHub.Global.Gerit.Infra.Job.Services;
using VianaHub.Global.Gerit.Infra.Job.HostedServices;
using VianaHub.Global.Gerit.Domain.Interfaces.Job;
using VianaHub.Global.Gerit.Infra.Job.Interfaces;
using VianaHub.Global.Gerit.Application.Services.Identity;
using VianaHub.Global.Gerit.Application.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Services.Identity;
using VianaHub.Global.Gerit.Infra.Data.Repository.Identity;
using VianaHub.Global.Gerit.Application.Interfaces.Billing;
using VianaHub.Global.Gerit.Application.Interfaces.Business;
using VianaHub.Global.Gerit.Application.Interfaces.Common;
using VianaHub.Global.Gerit.Application.Services.Billing;
using VianaHub.Global.Gerit.Application.Services.Business;
using VianaHub.Global.Gerit.Application.Services.Common;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Entities.Job;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Identity;
using VianaHub.Global.Gerit.Domain.Services.Billing;
using VianaHub.Global.Gerit.Domain.Services.Business;
using VianaHub.Global.Gerit.Domain.Validators.Billing.Plan;
using VianaHub.Global.Gerit.Domain.Validators.Billing.Subscription;
using VianaHub.Global.Gerit.Domain.Validators.Billing.Tenant;
using VianaHub.Global.Gerit.Domain.Validators.Identity.Action;
using VianaHub.Global.Gerit.Domain.Validators.Identity.Resource;
using VianaHub.Global.Gerit.Domain.Validators.Identity.Role;
using VianaHub.Global.Gerit.Domain.Validators.Identity.RolePermission;
using VianaHub.Global.Gerit.Domain.Validators.Identity.User;
using VianaHub.Global.Gerit.Domain.Validators.Identity.Jwt;
using VianaHub.Global.Gerit.Domain.Validators.Identity.UserRole;
using VianaHub.Global.Gerit.Domain.Validators.Business.Vehicle;
using VianaHub.Global.Gerit.Domain.Validators.Business.Equipment;
using VianaHub.Global.Gerit.Domain.Validators.Business.Function;
using VianaHub.Global.Gerit.Domain.Validators.Business.Team;
using VianaHub.Global.Gerit.Domain.Validators.Business.TeamMember;
using VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberAddress;
using VianaHub.Global.Gerit.Domain.Validators.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Domain.Validators.Business.Client;
using VianaHub.Global.Gerit.Domain.Validators.Business.ClientAddress;
using VianaHub.Global.Gerit.Domain.Validators.Business.ClientContact;
using VianaHub.Global.Gerit.Domain.Validators.Business.Status;
using VianaHub.Global.Gerit.Domain.Validators.Business.Intervention;
using VianaHub.Global.Gerit.Domain.Validators.Business.InterventionContact;
using VianaHub.Global.Gerit.Domain.Validators.Business.EquipmentType;
using VianaHub.Global.Gerit.Domain.Validators.Business.AddressType;
using VianaHub.Global.Gerit.Infra.Data.Repository.Business;
using VianaHub.Global.Gerit.Infra.Data.Repository.Billing;
using VianaHub.Global.Gerit.Infra.Data.Repository.Job;
using VianaHub.Global.Gerit.Infra.Job.Jobs.Maintenance;
using VianaHub.Global.Gerit.Infra.Job.Jobs.Security;
using VianaHub.Global.Gerit.Domain.Validators.Business.InterventionAddress;
using VianaHub.Global.Gerit.Domain.Validators.Business.StatusType;
using VianaHub.Global.Gerit.Domain.Validators.Business.TeamMembersTeam;
using VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeam;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamVehicle;
using VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeamEquipment;
using VianaHub.Global.Gerit.Domain.Validators.Identity.UserPreferences; // <-- added

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

        // Contexto de Tenant para requests não autenticados (ex: login, register)
        // Scoped: vive durante o ciclo de vida do request HTTP
        services.AddScoped<IRequestTenantContext, RequestTenantContext>();

        // Validators (Scoped)
        services.AddScoped<IValidator<UserRoleEntity>, UserRoleValidator>();
        services.AddScoped<IValidator<RolePermissionEntity>, RolePermissionValidator>();

        services.AddScoped<IEntityDomainValidator<ActionEntity>, ActionValidator>();
        services.AddScoped<IEntityDomainValidator<ResourceEntity>, ResourceValidator>();
        services.AddScoped<IEntityDomainValidator<RoleEntity>, RoleValidator>();
        services.AddScoped<IEntityDomainValidator<PlanEntity>, PlanValidator>();
        services.AddScoped<IEntityDomainValidator<TenantEntity>, TenantValidator>();
        services.AddScoped<IEntityDomainValidator<SubscriptionEntity>, SubscriptionValidator>();
        services.AddScoped<IEntityDomainValidator<UserEntity>, UserValidator>();
        services.AddScoped<IEntityDomainValidator<UserPreferencesEntity>, UserPreferencesValidator>();
        services.AddScoped<IEntityDomainValidator<JobDefinitionEntity>, JobDefinitionValidator>();
        services.AddScoped<IEntityDomainValidator<JwtKeyEntity>, JwtKeyValidator>();
        services.AddScoped<IEntityDomainValidator<VehicleEntity>, VehicleValidator>();
        services.AddScoped<IEntityDomainValidator<EquipmentEntity>, EquipmentValidator>();
        services.AddScoped<IEntityDomainValidator<EquipmentTypeEntity>, EquipmentTypeValidator>();
        services.AddScoped<IEntityDomainValidator<FunctionEntity>, FunctionValidator>();
        services.AddScoped<IEntityDomainValidator<TeamEntity>, TeamValidator>();
        services.AddScoped<IEntityDomainValidator<TeamMemberEntity>, TeamMemberValidator>();
        services.AddScoped<IEntityDomainValidator<TeamMemberAddressEntity>, TeamMemberAddressValidator>();
        services.AddScoped<IEntityDomainValidator<TeamMemberContactEntity>, TeamMemberContactValidator>();
        services.AddScoped<IEntityDomainValidator<AddressTypeEntity>, AddressTypeValidator>();
        services.AddScoped<IEntityDomainValidator<ClientEntity>, ClientValidator>();
        services.AddScoped<IEntityDomainValidator<ClientAddressEntity>, ClientAddressValidator>();
        services.AddScoped<IEntityDomainValidator<ClientContactEntity>, ClientContactValidator>();
        services.AddScoped<IEntityDomainValidator<InterventionContactEntity>, InterventionContactValidator>();
        services.AddScoped<IEntityDomainValidator<StatusEntity>, StatusValidator>();
        services.AddScoped<IEntityDomainValidator<InterventionEntity>, InterventionValidator>();
        services.AddScoped<IEntityDomainValidator<StatusTypeEntity>, StatusTypeValidator>();
        services.AddScoped<IEntityDomainValidator<InterventionAddressEntity>, InterventionAddressValidator>();
        services.AddScoped<IEntityDomainValidator<TeamMembersTeamEntity>, TeamMembersTeamValidator>();
        services.AddScoped<IEntityDomainValidator<InterventionTeamEntity>, InterventionTeamValidator>();
        services.AddScoped<IEntityDomainValidator<InterventionTeamVehicleEntity>, InterventionTeamVehicleValidator>();
        services.AddScoped<IEntityDomainValidator<InterventionTeamEquipmentEntity>, InterventionTeamEquipmentValidator>();

        // Application - Common Services
        services.AddScoped<IFileValidationService, FileValidationService>();

        // Application - App Services
        services.AddScoped<IAuthAppService, AuthAppService>();
        services.AddScoped<IUserRoleAppService, UserRoleAppService>();
        services.AddScoped<IJwtKeyAppService, JwtKeyAppService>();
        services.AddScoped<IAddressTypeAppService, AddressTypeAppService>();
        services.AddScoped<IFunctionAppService, FunctionAppService>();
        services.AddScoped<ITeamAppService, TeamAppService>();
        services.AddScoped<IVehicleAppService, VehicleAppService>();
        services.AddScoped<IEquipmentAppService, EquipmentAppService>();
        services.AddScoped<ITeamMemberAppService, TeamMemberAppService>();
        services.AddScoped<ITeamMembersTeamsAppService, TeamMembersTeamAppService>();
        services.AddScoped<ITeamMembersTeamDataRepository, TeamMembersTeamDataRepository>();
        services.AddScoped<IInterventionTeamsAppService, InterventionTeamAppService>();
        services.AddScoped<IInterventionTeamDataRepository, InterventionTeamDataRepository>();
        services.AddScoped<IClientAppService, ClientAppService>();
        services.AddScoped<IClientAddressAppService, ClientAddressAppService>();
        services.AddScoped<IClientContactAppService, ClientContactAppService>();
        services.AddScoped<IInterventionContactAppService, InterventionContactAppService>();
        services.AddScoped<IInterventionAddressAppService, InterventionAddressAppService>();
        services.AddScoped<IStatusAppService, StatusAppService>();
        services.AddScoped<IInterventionAppService, InterventionAppService>();
        services.AddScoped<IStatusTypeAppService, StatusTypeAppService>();
        services.AddScoped<IActionAppService, ActionAppService>();
        services.AddScoped<IResourceAppService, ResourceAppService>();
        services.AddScoped<IRoleAppService, RoleAppService>();
        services.AddScoped<IRolePermissionAppService, RolePermissionAppService>();
        services.AddScoped<IPlanAppService, PlanAppService>();
        services.AddScoped<ITenantAppService, TenantAppService>();
        services.AddScoped<ISubscriptionAppService, SubscriptionAppService>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IUserPreferencesAppService, UserPreferencesAppService>();

        // Domain
        services.AddScoped<IUserRoleDomainService, UserRoleDomainService>();
        services.AddScoped<IInterventionTeamDomainService, InterventionTeamDomainService>();
        services.AddScoped<IActionDomainService, ActionDomainService>();
        services.AddScoped<ITeamMembersTeamDomainService, TeamMembersTeamDomainService>();
        services.AddScoped<IResourceDomainService, ResourceDomainService>();
        services.AddScoped<IAddressTypeDomainService, AddressTypeDomainService>();
        services.AddScoped<IRoleDomainService, RoleDomainService>();
        services.AddScoped<IPlanDomainService, PlanDomainService>();
        services.AddScoped<ITenantDomainService, TenantDomainService>();
        services.AddScoped<ISubscriptionDomainService, SubscriptionDomainService>();
        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IUserPreferencesDomainService, UserPreferencesDomainService>();
        services.AddScoped<IRolePermissionDomainService, RolePermissionDomainService>();
        services.AddScoped<IJwtKeyDomainService, JwtKeyDomainService>();
        services.AddScoped<IFunctionDomainService, FunctionDomainService>();
        services.AddScoped<ITeamDomainService, TeamDomainService>();
        services.AddScoped<IVehicleDomainService, VehicleDomainService>();
        services.AddScoped<IEquipmentDomainService, EquipmentDomainService>();
        services.AddScoped<IEquipmentTypeDomainService, EquipmentTypeDomainService>();
        services.AddScoped<ITeamMemberDomainService, TeamMemberDomainService>();
        services.AddScoped<ITeamMemberAddressDomainService, TeamMemberAddressDomainService>();
        services.AddScoped<ITeamMemberContactDomainService, TeamMemberContactDomainService>();
        services.AddScoped<IClientDomainService, ClientDomainService>();
        services.AddScoped<IClientAddressDomainService, ClientAddressDomainService>();
        services.AddScoped<IClientContactDomainService, ClientContactDomainService>();
        services.AddScoped<IInterventionContactDomainService, InterventionContactDomainService>();
        services.AddScoped<IInterventionAddressDomainService, InterventionAddressDomainService>();
        services.AddScoped<IStatusDomainService, StatusDomainService>();
        services.AddScoped<IInterventionDomainService, InterventionDomainService>();
        services.AddScoped<IStatusTypeDomainService, StatusTypeDomainService>();
        services.AddScoped<IInterventionTeamVehicleDomainService, InterventionTeamVehicleDomainService>();
        services.AddScoped<IInterventionTeamEquipmentDomainService, InterventionTeamEquipmentDomainService>();

        // Infra.Data - Repositories
        services.AddScoped<IAddressTypeDataRepository, AddressTypeDataRepository>();
        services.AddScoped<IFunctionDataRepository, FunctionDataRepository>();
        services.AddScoped<ITeamDataRepository, TeamDataRepository>();
        services.AddScoped<IVehicleDataRepository, VehicleDataRepository>();
        services.AddScoped<IEquipmentDataRepository, EquipmentDataRepository>();
        services.AddScoped<IEquipmentTypeDataRepository, EquipmentTypeDataRepository>();
        services.AddScoped<ITeamMemberDataRepository, TeamMemberDataRepository>();
        services.AddScoped<ITeamMemberAddressDataRepository, TeamMemberAddressDataRepository>();
        services.AddScoped<ITeamMemberContactDataRepository, TeamMemberContactDataRepository>();
        services.AddScoped<IClientDataRepository, ClientDataRepository>();
        services.AddScoped<IClientAddressDataRepository, ClientAddressDataRepository>();
        services.AddScoped<IClientContactDataRepository, ClientContactDataRepository>();
        services.AddScoped<IInterventionContactDataRepository, InterventionContactDataRepository>();
        services.AddScoped<IInterventionAddressDataRepository, InterventionAddressDataRepository>();
        services.AddScoped<IStatusDataRepository, StatusDataRepository>();
        services.AddScoped<IInterventionDataRepository, InterventionDataRepository>();
        services.AddScoped<IStatusTypeDataRepository, StatusTypeDataRepository>();
        services.AddScoped<IActionDataRepository, ActionDataRepository>();
        services.AddScoped<IResourceDataRepository, ResourceDataRepository>();
        services.AddScoped<IRoleDataRepository, RoleDataRepository>();
        services.AddScoped<IPlanDataRepository, PlanDataRepository>();
        services.AddScoped<ITenantDataRepository, TenantDataRepository>();
        services.AddScoped<ISubscriptionDataRepository, SubscriptionDataRepository>();
        services.AddScoped<IJwtKeyDataRepository, JwtKeyDataRepository>();
        services.AddScoped<IUserDataRepository, UserDataRepository>();
        services.AddScoped<IUserPreferencesDataRepository, UserPreferencesDataRepository>();
        services.AddScoped<IUserRoleDataRepository, UserRoleDataRepository>();
        services.AddScoped<IRolePermissionDataRepository, RolePermissionDataRepository>();
        services.AddScoped<IRefreshTokenDataRepository, RefreshTokenDataRepository>();
        services.AddScoped<IJobDefinitionDataRepository, JobDefinitionDataRepository>();
        services.AddScoped<IInterventionTeamDataRepository, InterventionTeamDataRepository>();
        services.AddScoped<IInterventionTeamVehicleDataRepository, InterventionTeamVehicleDataRepository>();
        services.AddScoped<IInterventionTeamEquipmentDataRepository, InterventionTeamEquipmentDataRepository>();

        // Infra.Messaging (Email sender no-op por enquanto)
        services.AddScoped<IEmailSender, NoOpEmailSender>();

        // Hangfire Job service
        services.AddScoped<IJobSchedulerService, HangfireJobService>();
        services.AddScoped<IJobExecutor, HangfireJobExecutor>();
        services.AddScoped<IJobSyncService, JobSyncService>();
        services.AddScoped<ScheduledSyncJobDefinitionsJob>();
        services.AddScoped<JwtKeyRotationJob>();
        services.AddHostedService<JobSyncHostedService>();

        // Data Context
        services.AddScoped<GeritDbContext>();

        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Secret provider (chave mestra gerenciada externamente) - por padrão lê variável de ambiente JWT_MASTER_KEY
        services.AddSingleton<ISecretProvider, SecretProviderEnvironment>();

        return services;
    }
}

