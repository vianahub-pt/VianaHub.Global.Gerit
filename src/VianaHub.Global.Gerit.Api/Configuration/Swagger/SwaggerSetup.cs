using Microsoft.OpenApi.Models;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

public static class SwaggerSetup
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Bind Swagger settings from appsettings
        var swaggerSettings = new SwaggerSettings();
        configuration.GetSection("Swagger").Bind(swaggerSettings);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Define title and description based on environment
            string title;
            string description;

            if (environment.IsProduction())
            {
                title = "VianaHub Gerit API - Production";
                description = "Gerit Platform API for VianaHub - Production Environment";
            }
            else if (environment.IsStaging())
            {
                title = "VianaHub Gerit API - Staging";
                description = "Gerit Platform API for VianaHub - Staging Environment";
            }
            else
            {
                title = "VianaHub Gerit API - Development";
                description = "Gerit Platform API for VianaHub - Development Environment";
            }

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = title,
                Version = "v1",
                Description = description,
                Contact = new OpenApiContact
                {
                    Name = "VianaHub Support",
                    Email = "support@vianahub.pt",
                    Url = new Uri("https://vianahub.pt")
                },
                License = new OpenApiLicense
                {
                    Name = "Proprietary",
                    Url = new Uri("https://vianahub.pt/license")
                }
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
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
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

            // ✅ Adiciona suporte para upload de arquivos (IFormFile)
            options.OperationFilter<FileUploadOperationFilter>();

            // ✅ Adiciona o filtro de tradução do Swagger
            options.DocumentFilter<SwaggerTranslationFilter>();
        });

        return services;
    }

    /// <summary>
    /// Configura o Swagger UI com suporte a multi-idiomas
    /// </summary>
    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // ✅ Adiciona o middleware de localização do Swagger ANTES do UseSwagger
            app.UseMiddleware<Middleware.SwaggerLocalizationMiddleware>(
                new List<string> { "en-US", "pt-BR", "es-ES" }
            );

            app.UseSwagger(c =>
            {
                // Desabilita autenticação para o endpoint do Swagger em desenvolvimento
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    // Remove autenticação do próprio Swagger para evitar erro de chaves JWT
                    if (httpReq.Path.StartsWithSegments("/swagger"))
                    {
                        swaggerDoc.SecurityRequirements.Clear();
                    }
                });
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "VianaHub Gerit API v1");
                options.RoutePrefix = "swagger";
                options.DocumentTitle = "VianaHub Gerit API - Documentation";
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.ShowExtensions();
                options.EnableValidator();

                // ✅ Injeta os arquivos CSS e JS customizados para o seletor de idiomas
                options.InjectStylesheet("/swagger-ui/custom.css");
                options.InjectJavascript("/swagger-ui/custom.js");
            });
        }
    }
}
