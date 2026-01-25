using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Action;
using VianaHub.Global.Gerit.Application.Dtos.Request.Resource;
using VianaHub.Global.Gerit.Application.Dtos.Request.Role;
using VianaHub.Global.Gerit.Application.Dtos.Request.Plan;
using VianaHub.Global.Gerit.Application.Dtos.Request.Tenant;
using VianaHub.Global.Gerit.Application.Dtos.Request.User;
using VianaHub.Global.Gerit.Application.Dtos.Request.Subscription;
using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;

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
        services.AddScoped<IValidator<CreateActionRequest>, Validators.Action.CreateActionRouteValidator>();
        services.AddScoped<IValidator<UpdateActionRequest>, Validators.Action.UpdateActionRouteValidator>();

        // Resource Route Validators
        services.AddScoped<IValidator<CreateResourceRequest>, Validators.Resource.CreateResourceRouteValidator>();
        services.AddScoped<IValidator<UpdateResourceRequest>, Validators.Resource.UpdateResourceRouteValidator>();

        // Role Route Validators
        services.AddScoped<IValidator<CreateRoleRequest>, Validators.Role.CreateRoleRouteValidator>();
        services.AddScoped<IValidator<UpdateRoleRequest>, Validators.Role.UpdateRoleRouteValidator>();

        // Plan Route Validators
        services.AddScoped<IValidator<CreatePlanRequest>, Validators.Plan.CreatePlanRouteValidator>();
        services.AddScoped<IValidator<UpdatePlanRequest>, Validators.Plan.UpdatePlanRouteValidator>();

        // Tenant Route Validators
        services.AddScoped<IValidator<CreateTenantRequest>, Validators.Tenant.CreateTenantRouteValidator>();
        services.AddScoped<IValidator<UpdateTenantRequest>, Validators.Tenant.UpdateTenantRouteValidator>();

        // User Route Validators
        services.AddScoped<IValidator<CreateUserRequest>, Validators.User.CreateUserRouteValidator>();
        services.AddScoped<IValidator<UpdateUserRequest>, Validators.User.UpdateUserRouteValidator>();
        services.AddScoped<IValidator<UpdatePasswordRequest>, Validators.User.UpdatePasswordRouteValidator>();

        // Subscription Route Validators
        services.AddScoped<IValidator<CreateSubscriptionRequest>, Validators.Subscription.CreateSubscriptionRouteValidator>();
        services.AddScoped<IValidator<UpdateSubscriptionRequest>, Validators.Subscription.UpdateSubscriptionRouteValidator>();

        // Auth Route Validators
        services.AddScoped<IValidator<RegisterRequest>, Validators.Auth.RegisterRouteValidator>();
        services.AddScoped<IValidator<LoginRequest>, Validators.Auth.LoginRouteValidator>();
        services.AddScoped<IValidator<RefreshRequest>, Validators.Auth.RefreshRouteValidator>();

        return services;
    }
}
