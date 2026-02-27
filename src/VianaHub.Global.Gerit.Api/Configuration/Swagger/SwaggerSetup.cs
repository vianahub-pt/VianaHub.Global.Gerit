using Microsoft.OpenApi.Models;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

public static class SwaggerSetup
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Bind Swagger settings from appsettings
        var swaggerSettings = new SwaggerSettings();
        configuration.GetSection("Swagger").Bind(swaggerSettings);

        services.AddEndpointsApiExplorer();

        // Tenta obter o serviço de localização (caso já tenha sido registrado) para evitar strings hardcoded
        var provider = services.BuildServiceProvider();
        var localization = provider.GetService<ILocalizationService>();

        services.AddSwaggerGen(options =>
        {
            // Define title and description based on environment via chaves de tradução
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

            var title = localization?.GetMessage(titleKey) ?? titleKey;
            var description = localization?.GetMessage(descriptionKey) ?? descriptionKey;

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = title,
                Version = "v1",
                Description = description,
                Contact = new OpenApiContact
                {
                    // Nome do contato via chave de tradução
                    Name = localization?.GetMessage("Swagger.Api.Contact.Name") ?? "Swagger.Api.Contact.Name",
                    Email = "support@vianahub.pt",
                    Url = new Uri("https://vianahub.pt")
                },
                License = new OpenApiLicense
                {
                    Name = localization?.GetMessage("Swagger.Api.License.Name") ?? "Swagger.Api.License.Name",
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
                // Define a descrição como chave de tradução para que o SwaggerTranslationFilter faça a substituição
                Description = localization?.GetMessage("Swagger.Security.Bearer.Description") ?? "Swagger.Security.Bearer.Description"
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
        // ✅ Adiciona o middleware de localização do Swagger ANTES do UseSwagger
        app.UseMiddleware<Middleware.SwaggerLocalizationMiddleware>(
            new List<string> { "pt-PT", "en-US", "es-ES" }
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
            // Tenta obter o serviço de localização do provider da aplicação
            var localization = app.Services.GetService<ILocalizationService>();

            // Endpoint name (exibido no seletor) via tradução
            var endpointName = localization?.GetMessage("Swagger.UI.EndpointName", "v1") ?? "Swagger.UI.EndpointName";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", endpointName);
            options.RoutePrefix = "swagger";

            options.DocumentTitle = localization?.GetMessage("Swagger.UI.DocumentTitle") ?? "Swagger.UI.DocumentTitle";
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
