#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text.Json;

namespace VianaHub.Global.Gerit.Api.Configuration.Swagger;

/// <summary>
/// Document filter que traduz a documentação Swagger/OpenAPI baseado na cultura atual.
/// Carrega os ficheiros de localização da pasta `locales` para a cultura solicitada.
/// 
/// A cultura é obtida do HttpContext.Items["SwaggerCulture"] (definido pelo SwaggerLocalizationMiddleware),
/// com fallback para CultureInfo.CurrentUICulture.Name.
/// 
/// O cache é invalidado automaticamente quando os ficheiros de localização são alterados
/// ou quando uma cultura antes indisponível passa a ter traduções.
/// </summary>
public class SwaggerTranslationFilter : IDocumentFilter
{
    private static readonly Dictionary<string, Dictionary<string, string>> _translationsCache = new();
    private static readonly object _lock = new();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SwaggerTranslationFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        try
        {
            // Obtém a cultura do HttpContext.Items (definida pelo SwaggerLocalizationMiddleware)
            // Fallback para CurrentUICulture se o middleware não tiver definido
            var culture = GetCurrentCulture();

            Log.Debug("🔄 [Gerit:SwaggerTranslation] Applying translations for culture: {Culture}", culture);

            // Carrega as traduções a partir dos ficheiros JSON na pasta locales
            var translations = LoadTranslations(culture);
            if (translations == null || translations.Count == 0)
            {
                Log.Warning("⚠️ [Gerit:SwaggerTranslation] No translations found for culture: {Culture}", culture);

                // Tenta fallback para pt-PT se a cultura solicitada não for pt-PT
                if (!culture.Equals("pt-PT", StringComparison.OrdinalIgnoreCase))
                {
                    Log.Information("🔄 [Gerit:SwaggerTranslation] Trying fallback to pt-PT for culture: {Culture}", culture);
                    translations = LoadTranslations("pt-PT");
                }

                if (translations == null || translations.Count == 0)
                {
                    Log.Warning("⚠️ [Gerit:SwaggerTranslation] No fallback translations available. Raw keys will be shown.");
                    return;
                }
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

            Log.Information("✅ [Gerit:SwaggerTranslation] Successfully translated Swagger document to {Culture}", culture);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "❌ [Gerit:SwaggerTranslation] Error translating Swagger document");
        }
    }

    /// <summary>
    /// Obtém a cultura atual para tradução do Swagger.
    /// Prioridade: HttpContext.Items["SwaggerCulture"] (definido pelo middleware) > CurrentUICulture.Name.
    /// </summary>
    private string GetCurrentCulture()
    {
        // Tenta obter do HttpContext.Items (definido pelo SwaggerLocalizationMiddleware)
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Items.TryGetValue("SwaggerCulture", out var cultureObj) == true
            && cultureObj is string cultureName
            && !string.IsNullOrWhiteSpace(cultureName))
        {
            Log.Debug("🎯 [Gerit:SwaggerTranslation] Culture from HttpContext.Items: {Culture}", cultureName);
            return cultureName;
        }

        // Fallback para CurrentUICulture
        var fallbackCulture = CultureInfo.CurrentUICulture.Name;
        Log.Debug("🎯 [Gerit:SwaggerTranslation] Culture from CurrentUICulture (fallback): {Culture}", fallbackCulture);
        return fallbackCulture;
    }

    /// <summary>
    /// Carrega as traduções do ficheiro common.json para a cultura especificada.
    /// Apenas carregamentos bem-sucedidos são cacheados. Falhas são retentadas em cada pedido.
    /// </summary>
    private Dictionary<string, string>? LoadTranslations(string culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
            culture = CultureInfo.CurrentUICulture.Name;

        // Verifica cache (apenas loads bem-sucedidos são cacheados)
        if (_translationsCache.TryGetValue(culture, out var cached))
        {
            // Se o cache tem entradas, retorna. Se está vazio, pode ser uma falha anterior — retenta.
            if (cached.Count > 0)
            {
                return cached;
            }

            Log.Debug("🔄 [Gerit:SwaggerTranslation] Cache hit for {Culture} is empty, retrying load", culture);
        }

        lock (_lock)
        {
            // Double-check após lock
            if (_translationsCache.TryGetValue(culture, out cached) && cached.Count > 0)
                return cached;

            try
            {
                // Use AppContext.BaseDirectory para garantir que localizamos os arquivos na pasta de saída
                var basePath = AppContext.BaseDirectory;
                var localesPath = Path.Combine(basePath, "locales");

                Log.Debug("🔍 [Gerit:SwaggerTranslation] Looking for locales at: {Path}", localesPath);

                if (!Directory.Exists(localesPath))
                {
                    Log.Warning("[Gerit:SwaggerTranslation] Locales folder not found: {Path}. BaseDirectory: {BaseDir}",
                        localesPath, basePath);
                    // NÃO cachear — permite retentativa no próximo pedido
                    return null;
                }

                var merged = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // Carregar o ficheiro common.json para a cultura
                var commonFilePath = Path.Combine(localesPath, culture, "common.json");

                if (File.Exists(commonFilePath))
                {
                    try
                    {
                        var json = File.ReadAllText(commonFilePath);
                        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                        if (dict != null && dict.Count > 0)
                        {
                            foreach (var kvp in dict)
                            {
                                merged[kvp.Key] = kvp.Value;
                            }
                            Log.Debug("[Gerit:SwaggerTranslation] Loaded {Count} translations from {File}",
                                dict.Count, Path.GetFileName(commonFilePath));
                        }
                        else
                        {
                            Log.Warning("[Gerit:SwaggerTranslation] File {File} is empty or deserialized to null",
                                commonFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "[Gerit:SwaggerTranslation] Error deserializing {File}", commonFilePath);
                        // NÃO cachear — permite retentativa
                        return null;
                    }
                }
                else
                {
                    Log.Warning("[Gerit:SwaggerTranslation] Translation file not found: {Path}", commonFilePath);
                    // NÃO cachear — pode ser que a cultura não tenha traduções e precise de fallback
                    return null;
                }

                if (merged.Count == 0)
                {
                    Log.Warning("[Gerit:SwaggerTranslation] No translations loaded for culture: {Culture}", culture);
                    // NÃO cachear vazio — permite retentativa e fallback
                    return null;
                }

                // Só cachear carregamentos bem-sucedidos (com conteúdo)
                _translationsCache[culture] = merged;
                Log.Information("[Gerit:SwaggerTranslation] Successfully loaded and cached {Count} translations for culture {Culture}",
                    merged.Count, culture);
                return merged;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[Gerit:SwaggerTranslation] Error loading translations for culture: {Culture}", culture);
                // NÃO cachear falhas — permite retentativa no próximo pedido
                return null;
            }
        }
    }

    /// <summary>
    /// Limpa o cache de traduções. Útil para forçar o recarregamento
    /// após alterações nos ficheiros de localização sem reiniciar a aplicação.
    /// </summary>
    public static void ClearCache()
    {
        lock (_lock)
        {
            _translationsCache.Clear();
            Log.Information("[Gerit:SwaggerTranslation] Translation cache cleared");
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

        // Traduz usando o OperationId como chave
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
