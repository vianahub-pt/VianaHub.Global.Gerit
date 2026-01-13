using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace VianaHub.Global.Gerit.Api.Middleware;

/// <summary>
/// Middleware that sets culture for Swagger document generation based on ?lang= query parameter
/// or Accept-Language header. This middleware has PRIORITY over RequestLocalizationMiddleware
/// for Swagger routes.
/// </summary>
public class SwaggerLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IList<CultureInfo> _supportedCultures;

    public SwaggerLocalizationMiddleware(RequestDelegate next, IEnumerable<string>? supportedCultures = null)
    {
        _next = next;

        var list = supportedCultures?.ToList();
        if (list == null || list.Count == 0)
        {
            list = new List<string> { "en-US", "pt-BR", "es-ES" };
        }

        _supportedCultures = list.Select(c => new CultureInfo(c)).ToList();

    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only intercept Swagger routes (/swagger or /swagger/*)
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var culture = DetermineSwaggerCulture(context);

            // IMPORTANT: Set culture for the current async context
            // This ensures the culture is available when SwaggerGen executes
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // Store culture in HttpContext.Items for LocalizationService
            // This OVERRIDES the value set by RequestLocalizationMiddleware
            context.Items["RequestCulture"] = culture.Name;

        }

        await _next(context);
    }

    private CultureInfo DetermineSwaggerCulture(HttpContext context)
    {
        // Priority 1: Query parameter ?lang=xx-YY (HIGHEST PRIORITY for Swagger)
        if (context.Request.Query.TryGetValue("lang", out var langValue))
        {
            var requestedLang = langValue.ToString();
            var match = _supportedCultures.FirstOrDefault(c =>
                string.Equals(c.Name, requestedLang, StringComparison.OrdinalIgnoreCase));

            if (match != null)
                return match;
        }

        // Priority 2: Cookie (for Swagger UI persistence)
        if (context.Request.Cookies.TryGetValue("swagger-locale", out var cookieValue))
        {
            var match = _supportedCultures.FirstOrDefault(c =>
                string.Equals(c.Name, cookieValue, StringComparison.OrdinalIgnoreCase));

            if (match != null)
                return match;
        }

        // Priority 3: Accept-Language header
        if (context.Request.Headers.TryGetValue("Accept-Language", out StringValues values))
        {
            var accepted = values.ToString();
            var parts = accepted.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p));
            foreach (var part in parts)
            {
                var lang = part.Split(';')[0].Trim();

                // Try exact match
                var match = _supportedCultures.FirstOrDefault(s =>
                    string.Equals(s.Name, lang, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                    return match;

                // Try neutral match (language only)
                var neutral = _supportedCultures.FirstOrDefault(s =>
                    string.Equals(s.TwoLetterISOLanguageName, lang, StringComparison.OrdinalIgnoreCase));

                if (neutral != null)
                    return neutral;
            }
        }

        return new CultureInfo("pt-BR");
    }
}
