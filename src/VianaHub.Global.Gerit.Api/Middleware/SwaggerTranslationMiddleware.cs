#nullable enable

using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace VianaHub.Global.Gerit.Api.Middleware;

/// <summary>
/// Middleware que intercepta pedidos ao /swagger/{documentName}/swagger.json,
/// gera um documento OpenAPI FRESCO via ISwaggerProvider.GetSwagger() (bypassing o cache do Swashbuckle),
/// aplica traduções baseadas na cultura do pedido e retorna o JSON diretamente.
///
/// Motivação: O SwaggerMiddleware do Swashbuckle mantém uma ConcurrentDictionary de cache
/// que guarda o JSON serializado após a primeira chamada ao IDocumentFilter.Apply().
/// Isto significa que pedidos subsequentes recebem sempre o documento em cache,
/// independentemente do parâmetro ?lang= — o filtro de tradução nunca é executado novamente.
///
/// Esta middleware resolve o problema gerando um documento novo por pedido e aplicando
/// as traduções diretamente, ignorando completamente o pipeline de cache do Swashbuckle.
/// </summary>
public class SwaggerTranslationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _supportedCultures;
    private static readonly Dictionary<string, Dictionary<string, string>> _translationsCache = new();
    private static readonly object _lock = new();
    private static readonly Regex _swaggerJsonPattern = new(
        @"^/swagger/(?<documentName>[^/]+)/swagger\.json$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    public SwaggerTranslationMiddleware(RequestDelegate next)
    {
        _next = next;
        _supportedCultures = new List<string> { "pt-PT", "pt-BR", "en-US", "es-ES" };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var match = _swaggerJsonPattern.Match(path);

        // Só processa pedidos que correspondem ao padrão /swagger/{documentName}/swagger.json
        if (!match.Success)
        {
            await _next(context);
            return;
        }

        var documentName = match.Groups["documentName"].Value;

        // Determina a cultura a partir do pedido (query, cookie, header, fallback)
        var culture = GetCultureFromRequest(context);

        Log.Debug("🌐 [Gerit:SwaggerTranslationMiddleware] Intercepting Swagger JSON request: doc='{DocumentName}', culture='{Culture}'",
            documentName, culture);

        try
        {
            // Obtém o ISwaggerProvider registrado pelo AddSwaggerGen()
            var swaggerProvider = context.RequestServices.GetRequiredService<ISwaggerProvider>();

            // Gera um documento FRESCO — SEM passar pelo cache do SwaggerMiddleware
            var swaggerDoc = swaggerProvider.GetSwagger(documentName, null, null);

            if (swaggerDoc == null)
            {
                Log.Warning("[Gerit:SwaggerTranslationMiddleware] Swagger document '{DocumentName}' not found", documentName);
                context.Response.StatusCode = 404;
                return;
            }

            // Carrega e aplica traduções
            var translations = LoadTranslations(culture);
            if (translations != null && translations.Count > 0)
            {
                // Traduz as informações da API
                TranslateApiInfo(swaggerDoc.Info, translations);

                // Traduz todos os paths (endpoints)
                foreach (var pathItem in swaggerDoc.Paths)
                {
                    foreach (var operation in pathItem.Value.Operations.Values)
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

                Log.Information("✅ [Gerit:SwaggerTranslationMiddleware] Translations applied for '{DocumentName}' in '{Culture}'",
                    documentName, culture);
            }
            else
            {
                Log.Warning("⚠️ [Gerit:SwaggerTranslationMiddleware] No translations found for culture '{Culture}'", culture);
            }

            // Limpa security requirements (mesmo comportamento do PreSerializeFilter existente)
            swaggerDoc.SecurityRequirements.Clear();

            // Serializa o documento OpenAPI para JSON
            using var writer = new StringWriter();
            var jsonWriter = new OpenApiJsonWriter(writer);
            swaggerDoc.SerializeAsV3(jsonWriter);
            var json = writer.ToString();

            // Retorna o JSON diretamente — NÃO chama _next(context)
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(json);

            Log.Information("✅ [Gerit:SwaggerTranslationMiddleware] Successfully served translated Swagger JSON for '{DocumentName}' in '{Culture}'",
                documentName, culture);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "❌ [Gerit:SwaggerTranslationMiddleware] Error processing Swagger translation for '{DocumentName}'", documentName);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync("{\"error\":\"Internal server error while generating Swagger documentation\"}");
        }
    }

    /// <summary>
    /// Determina a cultura do pedido usando a seguinte prioridade:
    /// 1. Query string ?lang=XX
    /// 2. Cookie swagger-locale
    /// 3. Header Accept-Language
    /// 4. Fallback para pt-PT
    /// </summary>
    private string GetCultureFromRequest(HttpContext context)
    {
        // 1. Query string (?lang=pt-PT)
        if (context.Request.Query.TryGetValue("lang", out var langQuery))
        {
            var lang = langQuery.ToString();
            if (_supportedCultures.Contains(lang, StringComparer.OrdinalIgnoreCase))
            {
                return lang;
            }
        }

        // 2. Cookie 'swagger-locale' (definido pelo custom.js do Swagger UI)
        if (context.Request.Cookies != null && context.Request.Cookies.TryGetValue("swagger-locale", out var cookieLang))
        {
            var lang = cookieLang?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(lang) && _supportedCultures.Contains(lang, StringComparer.OrdinalIgnoreCase))
            {
                return lang;
            }
        }

        // 3. Header Accept-Language
        if (context.Request.Headers.TryGetValue("Accept-Language", out var acceptLanguage))
        {
            var languages = acceptLanguage.ToString().Split(',');
            foreach (var language in languages)
            {
                var lang = language.Split(';')[0].Trim();
                if (_supportedCultures.Contains(lang, StringComparer.OrdinalIgnoreCase))
                {
                    return lang;
                }
            }
        }

        // 4. Fallback para pt-PT
        return "pt-PT";
    }

    /// <summary>
    /// Carrega as traduções do ficheiro common.json para a cultura solicitada.
    /// Utiliza cache thread-safe por cultura.
    /// Fallback para pt-PT se não encontrar traduções para a cultura solicitada.
    /// </summary>
    private Dictionary<string, string>? LoadTranslations(string culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
            culture = "pt-PT";

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
                var basePath = AppContext.BaseDirectory;
                var localesPath = Path.Combine(basePath, "locales");

                if (!Directory.Exists(localesPath))
                {
                    Log.Warning("[Gerit:SwaggerTranslationMiddleware] Locales folder not found: {Path}", localesPath);
                    _translationsCache[culture] = new Dictionary<string, string>();
                    return _translationsCache[culture];
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
                            Log.Debug("[Gerit:SwaggerTranslationMiddleware] Loaded {Count} translations from {File}",
                                dict.Count, Path.GetFileName(commonFilePath));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "[Gerit:SwaggerTranslationMiddleware] Error loading {File}", commonFilePath);
                    }
                }

                if (merged.Count == 0)
                {
                    // Tentar fallback pt-PT
                    if (!culture.Equals("pt-PT", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Warning("[Gerit:SwaggerTranslationMiddleware] No translations for {Culture}, trying fallback pt-PT", culture);
                        var fallback = LoadTranslations("pt-PT");
                        _translationsCache[culture] = fallback ?? new Dictionary<string, string>();
                        return _translationsCache[culture];
                    }

                    Log.Warning("[Gerit:SwaggerTranslationMiddleware] No translation files found for culture: {Culture}", culture);
                    _translationsCache[culture] = new Dictionary<string, string>();
                    return _translationsCache[culture];
                }

                _translationsCache[culture] = merged;
                Log.Information("[Gerit:SwaggerTranslationMiddleware] Successfully loaded {Count} translations for culture {Culture}",
                    merged.Count, culture);
                return merged;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[Gerit:SwaggerTranslationMiddleware] Error loading translations for culture: {Culture}", culture);
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

    private static void TranslateOperation(OpenApiOperation operation, Dictionary<string, string> translations)
    {
        if (operation == null) return;

        // Traduz o Summary se começar com "Swagger."
        if (!string.IsNullOrEmpty(operation.Summary) && operation.Summary.StartsWith("Swagger."))
        {
            operation.Summary = GetTranslation(translations, operation.Summary, operation.Summary);
        }

        // Traduz Summary e Description via OperationId
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

    private static void TranslateSchema(OpenApiSchema schema, Dictionary<string, string> translations)
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

    private static void TranslateSecurityScheme(OpenApiSecurityScheme securityScheme, Dictionary<string, string> translations)
    {
        if (securityScheme == null) return;

        securityScheme.Description = GetTranslation(translations, "Swagger.Security.Bearer.Description", securityScheme.Description);
    }

    /// <summary>
    /// Obtém a tradução ou retorna o valor padrão se não encontrar
    /// </summary>
    private static string GetTranslation(Dictionary<string, string>? translations, string key, string? defaultValue)
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
