using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Auth;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Action;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Resource;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.Role;
using VianaHub.Global.Gerit.Application.Dtos.Request.Identity.User;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Plan;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Subscription;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;
using VianaHub.Global.Gerit.Api.Validators.Billing.Plan;
using VianaHub.Global.Gerit.Api.Validators.Billing.Subscription;
using VianaHub.Global.Gerit.Api.Validators.Billing.Tenant;
using VianaHub.Global.Gerit.Api.Validators.Identity.Action;
using VianaHub.Global.Gerit.Api.Validators.Identity.Resource;
using VianaHub.Global.Gerit.Api.Validators.Identity.Role;
using VianaHub.Global.Gerit.Api.Validators.Identity.User;
using VianaHub.Global.Gerit.Api.Validators.Identity.Auth;

// Vehicle DTOs & Validators
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Vehicle;

// Equipment DTOs & Validators
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;
using VianaHub.Global.Gerit.Api.Validators.Business.Equipment;
using VianaHub.Global.Gerit.Api.Validators.Business.Vehicle;

namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Classe responsável por registrar os validadores de rota na injeção de dependência.
/// </summary>
public static class RouteValidatorSetup
{
    /// <summary>
    /// Adiciona os validadores de rota à coleção de serviços.
    /// </summary>
    /// <param name="services">A coleção de serviços da aplicação.</param>
    /// <returns>A própria coleção de serviços para encadeamento.</returns>
    public static IServiceCollection AddRouteValidatorSetup(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Action Route Validators
        services.AddScoped<IValidator<CreateActionRequest>, CreateActionRouteValidator>();
        services.AddScoped<IValidator<UpdateActionRequest>, UpdateActionRouteValidator>();

        // Resource Route Validators
        services.AddScoped<IValidator<CreateResourceRequest>, CreateResourceRouteValidator>();
        services.AddScoped<IValidator<UpdateResourceRequest>, UpdateResourceRouteValidator>();

        // Role Route Validators
        services.AddScoped<IValidator<CreateRoleRequest>, CreateRoleRouteValidator>();
        services.AddScoped<IValidator<UpdateRoleRequest>, UpdateRoleRouteValidator>();

        // Plan Route Validators
        services.AddScoped<IValidator<CreatePlanRequest>, CreatePlanRouteValidator>();
        services.AddScoped<IValidator<UpdatePlanRequest>, UpdatePlanRouteValidator>();

        // Tenant Route Validators
        services.AddScoped<IValidator<CreateTenantRequest>, CreateTenantRouteValidator>();
        services.AddScoped<IValidator<UpdateTenantRequest>, UpdateTenantRouteValidator>();

        // User Route Validators
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRouteValidator>();
        services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRouteValidator>();
        services.AddScoped<IValidator<UpdatePasswordRequest>, UpdatePasswordRouteValidator>();

        // Subscription Route Validators
        services.AddScoped<IValidator<CreateSubscriptionRequest>, CreateSubscriptionRouteValidator>();
        services.AddScoped<IValidator<UpdateSubscriptionRequest>, UpdateSubscriptionRouteValidator>();

        // Auth Route Validators
        services.AddScoped<IValidator<RegisterRequest>, RegisterRouteValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRouteValidator>();
        services.AddScoped<IValidator<RefreshRequest>, RefreshRouteValidator>();

        // Vehicle Route Validators
        services.AddScoped<IValidator<CreateVehicleRequest>, CreateVehicleRouteValidator>();
        services.AddScoped<IValidator<UpdateVehicleRequest>, UpdateVehicleRouteValidator>();
        // Equipment Route Validators
        services.AddScoped<IValidator<CreateEquipmentRequest>, CreateEquipmentRouteValidator>();
        services.AddScoped<IValidator<UpdateEquipmentRequest>, UpdateEquipmentRouteValidator>();

        return services;
    }
}
