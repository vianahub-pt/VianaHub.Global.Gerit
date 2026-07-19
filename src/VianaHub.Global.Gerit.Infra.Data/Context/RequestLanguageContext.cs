using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Domain.Constants;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Infra.Data.Context;

/// <summary>
/// Implementação scoped do contexto de idioma por request.
/// Lê de HttpContext.Items["RequestCulture"] definido pelo RequestLocalizationMiddleware.
/// Fallback para SupportedLanguages.Default (pt-PT).
/// </summary>
public sealed class RequestLanguageContext : IRequestLanguageContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestLanguageContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string LanguageCode
    {
        get
        {
            var culture = _httpContextAccessor.HttpContext?.Items["RequestCulture"]?.ToString();

            if (string.IsNullOrEmpty(culture))
                return SupportedLanguages.Default;

            if (SupportedLanguages.Supported.Contains(culture, StringComparer.OrdinalIgnoreCase))
                return culture;

            return SupportedLanguages.Default;
        }
    }
}
