#nullable enable

using Serilog;
using System.Globalization;

namespace VianaHub.Global.Gerit.Api.Middleware;

/// <summary>
/// Middleware que captura a cultura da query string para tradução do Swagger.
/// Exemplo: /swagger/v1/swagger.json?lang=pt-PT
/// </summary>
public class SwaggerLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IList<string> _supportedCultures;

    public SwaggerLocalizationMiddleware(
        RequestDelegate next,
        IEnumerable<string>? supportedCultures = null)
    {
        _next = next;

        var list = supportedCultures?.ToList();
        if (list == null || list.Count == 0)
        {
            list = new List<string> { "pt-PT", "en-US", "es-ES" };
        }

        _supportedCultures = list;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Só processa se for requisição do Swagger
        var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
        if (path.Contains("/swagger"))
        {
            var requestedCulture = GetCultureFromRequest(context);
            if (!string.IsNullOrEmpty(requestedCulture))
            {
                try
                {
                    var culture = new CultureInfo(requestedCulture);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;

                    // Armazena no HttpContext.Items para uso no SwaggerTranslationFilter
                    context.Items["SwaggerCulture"] = requestedCulture;

                    Log.Debug("?? [Gerit:SwaggerLocalization] Culture set to: {Culture} for Swagger", requestedCulture);
                }
                catch (CultureNotFoundException ex)
                {
                    Log.Warning(ex, "?? [Gerit:SwaggerLocalization] Invalid culture requested: {Culture}", requestedCulture);
                }
            }
        }

        await _next(context);
    }

    private string GetCultureFromRequest(HttpContext context)
    {
        // 1. Tenta pegar da query string (?lang=pt-PT)
        if (context.Request.Query.TryGetValue("lang", out var langQuery))
        {
            var lang = langQuery.ToString();
            if (_supportedCultures.Contains(lang, StringComparer.OrdinalIgnoreCase))
            {
                return lang;
            }
        }

        // 1.5 Tenta pegar do cookie 'swagger-locale' (definido pelo custom.js)
        if (context.Request.Cookies != null && context.Request.Cookies.TryGetValue("swagger-locale", out var cookieLang))
        {
            var lang = cookieLang?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(lang) && _supportedCultures.Contains(lang, StringComparer.OrdinalIgnoreCase))
            {
                return lang;
            }
        }

        // 2. Tenta pegar do header Accept-Language
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

        // 3. Fallback para pt-PT
        return "pt-PT";
    }
}
