namespace VianaHub.Global.Gerit.Api.Configuration;

/// <summary>
/// Configuração de CORS (Cross-Origin Resource Sharing)
/// </summary>
public static class CorsSetup
{
    /// <summary>
    /// Adiciona configuração de CORS ao pipeline
    /// </summary>
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsConfig = configuration.GetSection("Cors");
        var enableCors = corsConfig.GetValue<bool>("EnableCors", true);

        if (!enableCors)
        {
            return services;
        }

        var policyName = corsConfig.GetValue<string>("PolicyName") ?? "VianaIDCorsPolicy";
        var allowedOrigins = corsConfig.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        var allowedMethods = corsConfig.GetSection("AllowedMethods").Get<string[]>() ?? Array.Empty<string>();
        var allowedHeaders = corsConfig.GetSection("AllowedHeaders").Get<string[]>() ?? Array.Empty<string>();
        var allowCredentials = corsConfig.GetValue<bool>("AllowCredentials", true);
        var maxAge = corsConfig.GetValue<int>("MaxAge", 600);

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                // Configurar origens permitidas
                if (allowedOrigins.Length == 0 || allowedOrigins.Contains("*"))
                {
                    // ATENÇÃO: Permitir qualquer origem é INSEGURO em produção
                    var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
                    if (env?.Equals("Production", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        throw new InvalidOperationException(
                            "? ERRO DE SEGURANÇA: CORS com origens '*' não é permitido em produção. " +
                            "Configure origens específicas em appsettings.Production.json");
                    }

                    policy.AllowAnyOrigin();
                }
                else
                {
                    policy.WithOrigins(allowedOrigins)
                          .SetIsOriginAllowedToAllowWildcardSubdomains();
                }

                // Configurar métodos permitidos
                if (allowedMethods.Length == 0 || allowedMethods.Contains("*"))
                {
                    policy.AllowAnyMethod();
                }
                else
                {
                    policy.WithMethods(allowedMethods);
                }

                // Configurar headers permitidos
                if (allowedHeaders.Length == 0 || allowedHeaders.Contains("*"))
                {
                    policy.AllowAnyHeader();
                }
                else
                {
                    policy.WithHeaders(allowedHeaders);
                }

                // Credenciais
                if (allowCredentials)
                {
                    policy.AllowCredentials();
                }

                // Headers expostos
                policy.WithExposedHeaders(
                    "X-Pagination",
                    "X-Total-Count",
                    "X-Rate-Limit-Remaining",
                    "X-Rate-Limit-Reset",
                    "Retry-After"
                );

                // Cache de preflight
                policy.SetPreflightMaxAge(TimeSpan.FromSeconds(maxAge));
            });
        });

        return services;
    }

    /// <summary>
    /// Usa o middleware de CORS
    /// </summary>
    public static IApplicationBuilder UseCorsConfiguration(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        var policyName = configuration.GetValue<string>("Cors:PolicyName") ?? "VianaIDCorsPolicy";
        app.UseCors(policyName);
        return app;
    }
}
