using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

/// <summary>
/// Document filter that translates Swagger/OpenAPI documentation based on the current culture
/// </summary>
public class SwaggerTranslationFilter : IDocumentFilter
{
    private readonly IServiceProvider _serviceProvider;

    public SwaggerTranslationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        try
        {
            // Create a scope to resolve the scoped services
            using var scope = _serviceProvider.CreateScope();
            var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();
            var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            // IMPORTANTE: Pegar cultura do HttpContext.Items (definido pelo SwaggerLocalizationMiddleware)
            var culture = GetCultureFromHttpContext(httpContextAccessor);

            // Apply the culture to the current thread so localization services that rely on CurrentUICulture work
            if (!string.IsNullOrEmpty(culture))
            {
                try
                {
                    var ci = CultureInfo.GetCultureInfo(culture);
                    CultureInfo.CurrentCulture = ci;
                    CultureInfo.CurrentUICulture = ci;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Invalid culture '{Culture}' - keeping current cultures", culture);
                }
            }

            // Translate API info
            TranslateApiInfo(swaggerDoc.Info, localizationService);

            // Translate all paths (endpoints)
            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations.Values)
                {
                    TranslateOperation(operation, localizationService);
                }
            }

            // Translate schemas
            if (swaggerDoc.Components?.Schemas != null)
            {
                foreach (var schema in swaggerDoc.Components.Schemas.Values)
                {
                    TranslateSchema(schema, localizationService);
                }
            }

            // Translate security schemes
            if (swaggerDoc.Components?.SecuritySchemes != null)
            {
                foreach (var securityScheme in swaggerDoc.Components.SecuritySchemes.Values)
                {
                    TranslateSecurityScheme(securityScheme, localizationService);
                }
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex, "❌ [SwaggerTranslationFilter] Error translating Swagger document");
        }
    }

    private string GetCultureFromHttpContext(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;

        // 1. Tentar pegar do HttpContext.Items (definido pelo SwaggerLocalizationMiddleware) - PRIORIDADE MÁXIMA
        if (httpContext?.Items.TryGetValue("RequestCulture", out var cultureFromContext) == true)
        {
            try
            {
                if (cultureFromContext is string cultureName)
                    return cultureName;

                if (cultureFromContext is CultureInfo ci)
                    return ci.Name;

                if (cultureFromContext is RequestCulture rc)
                    return rc.UICulture.Name;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error reading culture from HttpContext.Items");
            }
        }

        // 2. Fallback para query parameter
        var langQueryParam = httpContext?.Request.Query["lang"].ToString();
        if (!string.IsNullOrEmpty(langQueryParam))
            return langQueryParam;

        // 3. Fallback para CurrentUICulture (último recurso)
        var fallbackCulture = CultureInfo.CurrentUICulture.Name;
        return fallbackCulture;
    }

    private string SafeGetMessage(ILocalizationService localizationService, string key)
    {
        try
        {
            var result = localizationService.GetMessage(key);
            return result;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error getting message for key: {Key}", key);
            return key; // Return key as fallback
        }
    }

    private void TranslateApiInfo(OpenApiInfo info, ILocalizationService localizationService)
    {
        try
        {
            // Translate title - check if it's a translation key
            if (!string.IsNullOrEmpty(info.Title))
            {
                // Check if title is a translation key (starts with "Swagger.")
                if (info.Title.StartsWith("Swagger."))
                {
                    var titleTranslated = SafeGetMessage(localizationService, info.Title);

                    if (titleTranslated != info.Title)
                        info.Title = titleTranslated;
                }
                else
                {
                    // Try convention-based keys when title is pre-resolved (e.g. "IAM VianaID - Development")
                    var candidates = new[]
                    {
                        "Swagger.Api.Title",
                        "Swagger.Api.Title.Development",
                        "Swagger.Api.Title.Staging",
                        "Swagger.Api.Title.Production"
                    };

                    foreach (var candidate in candidates)
                    {
                        var translated = SafeGetMessage(localizationService, candidate);
                        if (translated != candidate)
                        {
                            info.Title = translated;
                            break;
                        }
                    }
                }
            }

            // Translate description
            if (!string.IsNullOrEmpty(info.Description))
            {
                if (info.Description.StartsWith("Swagger."))
                {
                    var descTranslated = SafeGetMessage(localizationService, info.Description);

                    if (descTranslated != info.Description)
                        info.Description = descTranslated;
                }
                else
                {
                    var candidates = new[]
                    {
                        "Swagger.Api.Description",
                        "Swagger.Api.Description.Development",
                        "Swagger.Api.Description.Staging",
                        "Swagger.Api.Description.Production"
                    };

                    foreach (var candidate in candidates)
                    {
                        var translated = SafeGetMessage(localizationService, candidate);
                        if (translated != candidate)
                        {
                            info.Description = translated;
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating API info");
        }
    }

    private void TranslateOperation(OpenApiOperation operation, ILocalizationService localizationService)
    {
        try
        {
            // Translate summary - check if it's a translation key
            if (!string.IsNullOrEmpty(operation.Summary))
            {
                // If summary looks like a key (starts with "Swagger."), translate it
                if (operation.Summary.StartsWith("Swagger."))
                {
                    var translated = SafeGetMessage(localizationService, operation.Summary);
                    // Only replace if translation was found (not the same as the key)
                    if (translated != operation.Summary)
                        operation.Summary = translated;
                }
            }

            // Translate description
            if (!string.IsNullOrEmpty(operation.Description))
            {
                if (operation.Description.StartsWith("Swagger."))
                {
                    var translated = SafeGetMessage(localizationService, operation.Description);
                    if (translated != operation.Description)
                    {
                        operation.Description = translated;
                    }
                }
            }

            // Translate parameters
            if (operation.Parameters != null)
            {
                foreach (var parameter in operation.Parameters)
                {
                    TranslateParameter(parameter, operation.OperationId, localizationService);
                }
            }

            // Translate request body
            if (operation.RequestBody != null)
            {
                TranslateRequestBody(operation.RequestBody, operation.OperationId, localizationService);
            }

            // Translate responses
            foreach (var response in operation.Responses)
            {
                TranslateResponse(response.Value, operation.OperationId, response.Key, localizationService);
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating operation {OperationId}", operation.OperationId);
        }
    }

    private void TranslateParameter(OpenApiParameter parameter, string operationId, ILocalizationService localizationService)
    {
        try
        {
            if (!string.IsNullOrEmpty(parameter.Description))
            {
                if (parameter.Description.StartsWith("Swagger."))
                {
                    var translated = SafeGetMessage(localizationService, parameter.Description);
                    if (translated != parameter.Description)
                    {
                        parameter.Description = translated;
                    }
                }
                else
                {
                    // Try to find translation by convention
                    var key = $"Swagger.Parameter.{operationId}.{parameter.Name}.Description";
                    var translated = SafeGetMessage(localizationService, key);
                    if (translated != key)
                    {
                        parameter.Description = translated;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating parameter {Parameter}", parameter.Name);
        }
    }

    private void TranslateRequestBody(OpenApiRequestBody requestBody, string operationId, ILocalizationService localizationService)
    {
        try
        {
            if (!string.IsNullOrEmpty(requestBody.Description))
            {
                if (requestBody.Description.StartsWith("Swagger."))
                {
                    var translated = SafeGetMessage(localizationService, requestBody.Description);
                    if (translated != requestBody.Description)
                    {
                        requestBody.Description = translated;
                    }
                }
                else
                {
                    var key = $"Swagger.RequestBody.{operationId}.Description";
                    var translated = SafeGetMessage(localizationService, key);
                    if (translated != key)
                    {
                        requestBody.Description = translated;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating request body");
        }
    }

    private void TranslateResponse(OpenApiResponse response, string operationId, string statusCode, ILocalizationService localizationService)
    {
        try
        {
            if (!string.IsNullOrEmpty(response.Description))
            {
                // Check if description is a translation key
                if (response.Description.StartsWith("Swagger."))
                {
                    var translated = SafeGetMessage(localizationService, response.Description);
                    if (translated != response.Description)
                    {
                        response.Description = translated;
                    }
                }
                else
                {
                    // Try endpoint-specific response description
                    var key = $"Swagger.Response.{operationId}.{statusCode}.Description";
                    var translated = SafeGetMessage(localizationService, key);
                    if (translated != key)
                    {
                        response.Description = translated;
                    }
                    else
                    {
                        // Fallback to generic status code descriptions
                        var genericKey = $"Swagger.Response.{statusCode}";
                        translated = SafeGetMessage(localizationService, genericKey);
                        if (translated != genericKey)
                        {
                            response.Description = translated;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating response {StatusCode}", statusCode);
        }
    }

    private void TranslateSchema(OpenApiSchema schema, ILocalizationService localizationService)
    {
        try
        {
            // Translate schema description
            if (!string.IsNullOrEmpty(schema.Description))
            {
                if (schema.Description.StartsWith("Swagger."))
                {
                    var translated = SafeGetMessage(localizationService, schema.Description);
                    if (translated != schema.Description)
                    {
                        schema.Description = translated;
                    }
                }
            }

            // Translate property descriptions
            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties)
                {
                    if (!string.IsNullOrEmpty(property.Value.Description))
                    {
                        if (property.Value.Description.StartsWith("Swagger."))
                        {
                            var translated = SafeGetMessage(localizationService, property.Value.Description);
                            if (translated != property.Value.Description)
                            {
                                property.Value.Description = translated;
                            }
                        }
                        else
                        {
                            var key = $"Swagger.Property.{property.Key}.Description";
                            var translated = SafeGetMessage(localizationService, key);
                            if (translated != key)
                            {
                                property.Value.Description = translated;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating schema");
        }
    }

    private void TranslateSecurityScheme(OpenApiSecurityScheme securityScheme, ILocalizationService localizationService)
    {
        try
        {
            if (!string.IsNullOrEmpty(securityScheme.Description))
            {
                var key = "Swagger.Security.Bearer.Description";
                var translated = SafeGetMessage(localizationService, key);
                if (translated != key)
                {
                    securityScheme.Description = translated;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "⚠️ [SwaggerTranslationFilter] Error translating security scheme");
        }
    }
}
