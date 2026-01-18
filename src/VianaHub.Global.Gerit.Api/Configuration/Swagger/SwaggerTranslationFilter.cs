#nullable enable

using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text.Json;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

/// <summary>
/// Document filter que traduz a documentação Swagger/OpenAPI baseado na cultura atual.
/// Usa arquivos JSON estáticos de localização (messages.{culture}.json).
/// </summary>
public class SwaggerTranslationFilter : IDocumentFilter
{
    private static readonly Dictionary<string, Dictionary<string, string>> _translationsCache = new();
    private static readonly object _lock = new();

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        try
        {
            // Pega a cultura do CurrentUICulture (definida pelo SwaggerLocalizationMiddleware)
            var culture = CultureInfo.CurrentUICulture.Name;

            Log.Debug("?? [Gerit:SwaggerTranslation] Applying translations for culture: {Culture}", culture);

            // Carrega as traduções do arquivo JSON
            var translations = LoadTranslations(culture);
            if (translations == null || translations.Count == 0)
            {
                Log.Warning("?? [Gerit:SwaggerTranslation] No translations found for culture: {Culture}", culture);
                return;
            }

            // Traduz as informações da API
            TranslateApiInfo(swaggerDoc.Info, translations);

            // Traduz todos os paths (endpoints)
            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations.Values)
                {
                    TranslateOperation(operation, translations);
                }
            }

            // Traduz schemas
            if (swaggerDoc.Components?.Schemas != null)
            {
                foreach (var schema in swaggerDoc.Components.Schemas.Values)
                {
                    TranslateSchema(schema, translations);
                }
            }

            // Traduz security schemes
            if (swaggerDoc.Components?.SecuritySchemes != null)
            {
                foreach (var securityScheme in swaggerDoc.Components.SecuritySchemes.Values)
                {
                    TranslateSecurityScheme(securityScheme, translations);
                }
            }

            Log.Information("? [Gerit:SwaggerTranslation] Successfully translated Swagger document to {Culture}", culture);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "? [Gerit:SwaggerTranslation] Error translating Swagger document");
        }
    }

    /// <summary>
    /// Carrega as traduções do arquivo JSON com cache
    /// </summary>
    private Dictionary<string, string>? LoadTranslations(string culture)
    {
        // Verifica cache primeiro
        if (_translationsCache.TryGetValue(culture, out var cached))
        {
            return cached;
        }

        lock (_lock)
        {
            // Double-check após lock
            if (_translationsCache.TryGetValue(culture, out cached))
            {
                return cached;
            }

            try
            {
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Localization",
                    $"messages.{culture}.json"
                );

                if (!File.Exists(filePath))
                {
                    Log.Warning("?? [Gerit:SwaggerTranslation] Translation file not found: {FilePath}", filePath);
                    return null;
                }

                var json = File.ReadAllText(filePath);
                var translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (translations != null)
                {
                    _translationsCache[culture] = translations;
                    Log.Debug("? [Gerit:SwaggerTranslation] Loaded {Count} translations for {Culture}", translations.Count, culture);
                }

                return translations;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "? [Gerit:SwaggerTranslation] Error loading translations for culture: {Culture}", culture);
                return null;
            }
        }
    }

    private void TranslateApiInfo(OpenApiInfo info, Dictionary<string, string> translations)
    {
        if (info == null) return;

        info.Title = GetTranslation(translations, "Swagger.Api.Title", info.Title);
        info.Description = GetTranslation(translations, "Swagger.Api.Description", info.Description);

        if (info.Contact != null)
        {
            info.Contact.Name = GetTranslation(translations, "Swagger.Api.Contact.Name", info.Contact.Name);
        }

        if (info.License != null)
        {
            info.License.Name = GetTranslation(translations, "Swagger.Api.License.Name", info.License.Name);
        }
    }

    private void TranslateOperation(OpenApiOperation operation, Dictionary<string, string> translations)
    {
        if (operation == null) return;

        // Traduz o Summary se começar com "Swagger."
        if (!string.IsNullOrEmpty(operation.Summary) && operation.Summary.StartsWith("Swagger."))
        {
            operation.Summary = GetTranslation(translations, operation.Summary, operation.Summary);
        }

        // Traduz o OperationId
        operation.Summary = GetTranslation(translations, $"Swagger.Endpoint.{operation.OperationId}.Summary", operation.Summary);
        operation.Description = GetTranslation(translations, $"Swagger.Endpoint.{operation.OperationId}.Description", operation.Description);

        // Traduz parâmetros
        if (operation.Parameters != null)
        {
            foreach (var param in operation.Parameters)
            {
                param.Description = GetTranslation(translations, $"Swagger.Parameter.{param.Name}.Description", param.Description);
            }
        }

        // Traduz respostas
        if (operation.Responses != null)
        {
            foreach (var response in operation.Responses)
            {
                response.Value.Description = GetTranslation(translations, $"Swagger.Response.{response.Key}.Description", response.Value.Description);
            }
        }

        // Traduz tags
        if (operation.Tags != null)
        {
            for (int i = 0; i < operation.Tags.Count; i++)
            {
                var tag = operation.Tags[i];
                var translatedName = GetTranslation(translations, $"Swagger.Tag.{tag.Name}", tag.Name);
                if (translatedName != tag.Name)
                {
                    operation.Tags[i] = new OpenApiTag { Name = translatedName };
                }
            }
        }
    }

    private void TranslateSchema(OpenApiSchema schema, Dictionary<string, string> translations)
    {
        if (schema == null) return;

        schema.Description = GetTranslation(translations, $"Swagger.Schema.{schema.Title}.Description", schema.Description);

        // Traduz propriedades do schema
        if (schema.Properties != null)
        {
            foreach (var prop in schema.Properties)
            {
                prop.Value.Description = GetTranslation(translations, $"Swagger.Property.{prop.Key}.Description", prop.Value.Description);
            }
        }
    }

    private void TranslateSecurityScheme(OpenApiSecurityScheme securityScheme, Dictionary<string, string> translations)
    {
        if (securityScheme == null) return;

        securityScheme.Description = GetTranslation(translations, "Swagger.Security.Bearer.Description", securityScheme.Description);
    }

    /// <summary>
    /// Obtém a tradução ou retorna o valor padrão se não encontrar
    /// </summary>
    private string GetTranslation(Dictionary<string, string> translations, string key, string? defaultValue)
    {
        if (string.IsNullOrEmpty(key) || translations == null)
        {
            return defaultValue ?? string.Empty;
        }

        if (translations.TryGetValue(key, out var translation) && !string.IsNullOrEmpty(translation))
        {
            return translation;
        }

        return defaultValue ?? string.Empty;
    }
}
