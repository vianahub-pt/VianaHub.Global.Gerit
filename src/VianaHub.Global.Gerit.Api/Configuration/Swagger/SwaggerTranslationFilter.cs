#nullable enable

using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text.Json;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

/// <summary>
/// Document filter que traduz a documentação Swagger/OpenAPI baseado na cultura atual.
/// Carrega todos os arquivos de localização presentes na pasta `Localization` recursivamente e mescla as chaves para a cultura solicitada.
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

            // Carrega as traduções mescladas de todos os arquivos JSON na pasta Localization
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
    /// Carrega as traduções procurando recursivamente por arquivos '*.{culture}.json' dentro da pasta 'Localization' e mescla-os.
    /// Utiliza cache por cultura.
    /// </summary>
    private Dictionary<string, string>? LoadTranslations(string culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
            culture = CultureInfo.CurrentUICulture.Name;

        if (_translationsCache.TryGetValue(culture, out var cached))
        {
            return cached;
        }

        lock (_lock)
        {
            if (_translationsCache.TryGetValue(culture, out cached))
                return cached;

            try
            {
                // Use AppContext.BaseDirectory para garantir que localizamos os arquivos na pasta de saída
                var basePath = AppContext.BaseDirectory;
                var localizationPath = Path.Combine(basePath, "Localization");

                if (!Directory.Exists(localizationPath))
                {
                    Log.Warning("?? [Gerit:SwaggerTranslation] Localization folder not found: {Path}", localizationPath);
                    _translationsCache[culture] = new Dictionary<string, string>();
                    return _translationsCache[culture];
                }

                var merged = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var duplicateKeys = new List<string>();

                // Tentar cadeia de culturas: ex: "pt-PT", "pt", etc.
                var tryCultures = new[] {
                    culture,
                    // Surround with try/catch for segurança caso a cultura seja inválida
                    CultureInfo.GetCultureInfo(culture).Name,
                    CultureInfo.GetCultureInfo(culture).TwoLetterISOLanguageName
                }.Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                foreach (var c in tryCultures)
                {
                    // Buscar todos os arquivos que terminam com .{c}.json recursivamente
                    var pattern = $"*.{c}.json";
                    var files = Directory.GetFiles(localizationPath, pattern, SearchOption.AllDirectories);

                    if (files.Length == 0)
                    {
                        Log.Debug("?? [Gerit:SwaggerTranslation] No files for pattern {Pattern} in {Path}", pattern, localizationPath);
                        continue;
                    }

                    Log.Debug("?? [Gerit:SwaggerTranslation] Found {Count} files for culture token {CultureToken}", files.Length, c);

                    foreach (var file in files)
                    {
                        try
                        {
                            var json = File.ReadAllText(file);
                            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                            if (dict == null || dict.Count == 0)
                            {
                                Log.Debug("?? [Gerit:SwaggerTranslation] File {File} is empty or invalid", Path.GetFileName(file));
                                continue;
                            }

                            foreach (var kvp in dict)
                            {
                                if (merged.ContainsKey(kvp.Key))
                                {
                                    duplicateKeys.Add(kvp.Key);
                                    // Não sobrescrever, manter primeira ocorrência (prioridade por ordem de descoberta)
                                }
                                else
                                {
                                    merged[kvp.Key] = kvp.Value;
                                }
                            }

                            Log.Debug("? [Gerit:SwaggerTranslation] Loaded {Count} messages from {File}", dict.Count, Path.GetFileName(file));
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "? [Gerit:SwaggerTranslation] Error loading translation file {File}", file);
                        }
                    }

                    // Se já carregou chaves para a cultura exata, podemos parar antes de tentar a versão de duas letras
                    if (merged.Count > 0)
                        break;
                }

                if (merged.Count == 0)
                {
                    // Tentar fallback pt-PT
                    if (!culture.Equals("pt-PT", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Warning("?? [Gerit:SwaggerTranslation] No translations for {Culture}, trying fallback pt-PT", culture);
                        var fallback = LoadTranslations("pt-PT");
                        _translationsCache[culture] = fallback ?? new Dictionary<string, string>();
                        return _translationsCache[culture];
                    }

                    Log.Warning("?? [Gerit:SwaggerTranslation] No translation files found for culture chain: {Culture}", culture);
                    _translationsCache[culture] = new Dictionary<string, string>();
                    return _translationsCache[culture];
                }

                if (duplicateKeys.Count > 0)
                {
                    Log.Warning("?? [Gerit:SwaggerTranslation] Found {Count} duplicate keys while merging translations: {Keys}", duplicateKeys.Count, string.Join(", ", duplicateKeys.Distinct()));
                }

                _translationsCache[culture] = merged;
                Log.Information("? [Gerit:SwaggerTranslation] Successfully loaded {Count} translations for culture {Culture}", merged.Count, culture);
                return merged;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "? [Gerit:SwaggerTranslation] Error loading translations for culture: {Culture}", culture);
                _translationsCache[culture] = new Dictionary<string, string>();
                return _translationsCache[culture];
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

        // Tenta usar Title, se não existir usa Reference.Id (quando schemas são referenciados)
        var schemaId = schema.Title ?? schema.Reference?.Id;
        if (!string.IsNullOrEmpty(schemaId))
        {
            schema.Description = GetTranslation(translations, $"Swagger.Schema.{schemaId}.Description", schema.Description);
        }

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
