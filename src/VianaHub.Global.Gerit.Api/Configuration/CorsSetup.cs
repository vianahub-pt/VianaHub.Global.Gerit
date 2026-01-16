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

        var policyName = corsConfig.GetValue<string>("PolicyName") ?? "GeritCorsPolicy";
        var allowedOrigins = corsConfig.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        var allowedMethods = corsConfig.GetSection("AllowedMethods").Get<string[]>() ?? Array.Empty<string>();
        var allowedHeaders = corsConfig.GetSection("AllowedHeaders").Get<string[]>() ?? Array.Empty<string>();
        var allowCredentials = corsConfig.GetValue<bool?>("AllowCredentials");
        var maxAge = corsConfig.GetValue<int>("MaxAge", 600);

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                // Detectar se é ambiente de desenvolvimento
                var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
                var isDevelopment = env?.Equals("Development", StringComparison.OrdinalIgnoreCase) == true;
                
                // Configurar origens permitidas
                var allowAnyOrigin = allowedOrigins.Length == 0 || 
                                   allowedOrigins.Any(o => o.Equals("*", StringComparison.OrdinalIgnoreCase));

                if (allowAnyOrigin)
                {
                    // ATENÇÃO: Permitir qualquer origem é INSEGURO em produção
                    if (env?.Equals("Production", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        throw new InvalidOperationException(
                            "🔒 ERRO DE SEGURANÇA: CORS com origens '*' não é permitido em produção. " +
                            "Configure origens específicas em appsettings.Production.json");
                    }

                    policy.AllowAnyOrigin();
                    
                    // Quando AllowAnyOrigin() é usado, não podemos usar AllowCredentials()
                    // Isso é uma restrição de segurança do CORS
                }
                else
                {
                    policy.WithOrigins(allowedOrigins)
                          .SetIsOriginAllowedToAllowWildcardSubdomains();
                    
                    // Credenciais: só permitir se configurado explicitamente ou se não for AllowAnyOrigin
                    if (allowCredentials.HasValue ? allowCredentials.Value : true)
                    {
                        policy.AllowCredentials();
                    }
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
        var policyName = configuration.GetValue<string>("Cors:PolicyName") ?? "GeritCorsPolicy";
        app.UseCors(policyName);
        return app;
    }
}
