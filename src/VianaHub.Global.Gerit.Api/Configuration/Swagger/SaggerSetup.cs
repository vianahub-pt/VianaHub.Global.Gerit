using Microsoft.OpenApi.Models;
using VianaHub.Global.Gerit.Api.Middleware;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

public static class SaggerSetup
{
    public static IServiceCollection AddSagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Bind Swagger settings from appsettings
        var swaggerSettings = new SwaggerSettings();
        configuration.GetSection("Swagger").Bind(swaggerSettings);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Define translation keys based on environment
            // These will be translated by SwaggerTranslationFilter
            string titleKey;
            string descriptionKey;

            if (environment.IsProduction())
            {
                titleKey = "Swagger.Api.Title.Production";
                descriptionKey = "Swagger.Api.Description.Production";
            }
            else if (environment.IsStaging())
            {
                titleKey = "Swagger.Api.Title.Staging";
                descriptionKey = "Swagger.Api.Description.Staging";
            }
            else
            {
                titleKey = "Swagger.Api.Title.Development";
                descriptionKey = "Swagger.Api.Description.Development";
            }

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = titleKey,  // Translation key, will be translated by SwaggerTranslationFilter
                Version = "v1",
                Description = descriptionKey  // Translation key, will be translated by SwaggerTranslationFilter
            });

            options.UseAllOfToExtendReferenceSchemas();
            options.CustomSchemaIds(type => (type.FullName ?? type.Name).Replace('.', '_'));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Swagger.Security.Bearer.Description" // Translation key
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            options.EnableAnnotations();

            // Add localization filter using factory to properly resolve scoped service
            options.DocumentFilter<SwaggerTranslationFilter>();
        });

        return services;
    }

    /// <summary>
    /// Configura o middleware de autenticação e autorização no pipeline da aplicação.
    /// </summary>
    /// <param name="app">Builder da aplicação.</param>
    public static void UseSagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Add Swagger localization middleware before UseSwagger
            app.UseMiddleware<SwaggerLocalizationMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Inject custom HTML to add language switcher
                options.InjectJavascript("/swagger-ui/custom.js");
                options.InjectStylesheet("/swagger-ui/custom.css");

                // DEFAULT: Portuguese (pt-BR) listed first
                options.SwaggerEndpoint("/swagger/v1/swagger.json?lang=pt-BR", "IAM VianaID v1");
            });
        }
    }
}
