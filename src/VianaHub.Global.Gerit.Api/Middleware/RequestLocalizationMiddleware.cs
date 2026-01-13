using Microsoft.Extensions.Primitives;
using Serilog;
using System.Globalization;

namespace VianaHub.Global.Gerit.Api.Middleware;

/// <summary>
/// Middleware que define a cultura da aplicação com base no header Accept-Language.
/// Faz fallback para en-US quando não houver match.
/// </summary>
public class RequestLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IList<CultureInfo> _supported;

    public RequestLocalizationMiddleware(RequestDelegate next, IEnumerable<string>? supportedCultures = null)
    {
        _next = next;

        // Handle both null and empty collections
        var list = supportedCultures?.ToList();
        if (list == null || list.Count == 0)
        {
            list = new List<string> { "en-US", "pt-BR", "es-ES" };
        }

        _supported = list.Select(c => new CultureInfo(c)).ToList();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        CultureInfo culture;
        try
        {
            culture = DetermineRequestCulture(context.Request.Headers);
        }
        catch (Exception ex)
        {
            culture = new CultureInfo("en-US");
            Log.Warning(ex, "?? [RequestLocalization] Error determining culture, using fallback: en-US");
        }

        // Set culture for the current thread and async context
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        // Store culture in HttpContext.Items for access in async operations
        context.Items["RequestCulture"] = culture.Name;
        await _next(context);
    }

    private CultureInfo DetermineRequestCulture(IHeaderDictionary headers)
    {
        if (headers.TryGetValue("Accept-Language", out StringValues values))
        {
            var accepted = values.ToString();

            // Parse comma separated list with optional quality values
            // e.g. "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7"
            var parts = accepted.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p));
            foreach (var part in parts)
            {
                var lang = part.Split(';')[0].Trim();

                // Try exact match first
                var match = _supported.FirstOrDefault(s => string.Equals(s.Name, lang, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                    return match;

                // Try neutral match (language only)
                var neutral = _supported.FirstOrDefault(s => string.Equals(s.TwoLetterISOLanguageName, lang, StringComparison.OrdinalIgnoreCase));
                if (neutral != null)
                    return neutral;
            }
        }
        return new CultureInfo("en-US");
    }
}
