namespace VianaHub.Global.Gerit.Domain.Interfaces.Base;

/// <summary>
/// Contexto de idioma para o request atual (scoped).
/// Lê a cultura definida pelo RequestLocalizationMiddleware e armazenada em HttpContext.Items["RequestCulture"].
/// </summary>
public interface IRequestLanguageContext
{
    /// <summary>
    /// Código do idioma atual (ex: pt-PT, en-US, es-ES).
    /// </summary>
    string LanguageCode { get; }
}
