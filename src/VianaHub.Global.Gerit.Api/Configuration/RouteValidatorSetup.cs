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

        // ActionEntity Route Validators
        //services.AddScoped<IValidator<CreateActionRequest>, Validators.ActionEntity.CreateActionRouteValidator>();
        //services.AddScoped<IValidator<UpdateActionRequest>, Validators.ActionEntity.UpdateActionRouteValidator>();


        return services;
    }
}
