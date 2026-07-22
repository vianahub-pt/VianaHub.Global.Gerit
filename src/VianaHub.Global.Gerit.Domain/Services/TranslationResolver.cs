using VianaHub.Global.Gerit.Domain.Constants;

namespace VianaHub.Global.Gerit.Domain.Services;

/// <summary>
/// Utilitário genérico para resolver traduções a partir de coleções de entidades de tradução.
/// Procura primeiro a tradução no idioma solicitado; se não encontrar, faz fallback para o idioma padrão (pt-PT).
/// </summary>
public static class TranslationResolver
{
    /// <summary>
    /// Resolve o nome traduzido a partir de uma coleção de traduções genéricas.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade de tradução.</typeparam>
    /// <param name="translations">Coleção de traduções.</param>
    /// <param name="languageCode">Código do idioma desejado.</param>
    /// <param name="languageSelector">Função para extrair o LanguageCode da tradução.</param>
    /// <param name="nameSelector">Função para extrair o Name da tradução.</param>
    /// <returns>Nome traduzido ou null se não houver traduções.</returns>
    public static string? Resolve<T>(
        IEnumerable<T>? translations,
        string languageCode,
        Func<T, string?> languageSelector,
        Func<T, string?> nameSelector)
    {
        if (translations == null)
            return null;

        // Tenta encontrar tradução no idioma solicitado
        var match = translations.FirstOrDefault(t =>
            string.Equals(languageSelector(t), languageCode, StringComparison.OrdinalIgnoreCase));

        if (match != null)
        {
            var name = nameSelector(match);
            if (name != null)
                return name;
        }

        // Fallback para o idioma padrão (pt-PT)
        var fallback = translations.FirstOrDefault(t =>
            string.Equals(languageSelector(t), SupportedLanguages.Default, StringComparison.OrdinalIgnoreCase));

        return fallback != null ? nameSelector(fallback) : null;
    }

    /// <summary>
    /// Resolve uma descrição traduzida a partir de uma coleção de traduções genéricas.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade de tradução.</typeparam>
    /// <param name="translations">Coleção de traduções.</param>
    /// <param name="languageCode">Código do idioma desejado.</param>
    /// <param name="languageSelector">Função para extrair o LanguageCode da tradução.</param>
    /// <param name="descriptionSelector">Função para extrair a Description da tradução.</param>
    /// <returns>Descrição traduzida ou null se não houver traduções.</returns>
    public static string? ResolveDescription<T>(
        IEnumerable<T>? translations,
        string languageCode,
        Func<T, string?> languageSelector,
        Func<T, string?> descriptionSelector)
    {
        if (translations == null)
            return null;

        var match = translations.FirstOrDefault(t =>
            string.Equals(languageSelector(t), languageCode, StringComparison.OrdinalIgnoreCase));

        if (match != null)
        {
            var desc = descriptionSelector(match);
            if (desc != null)
                return desc;
        }

        var fallback = translations.FirstOrDefault(t =>
            string.Equals(languageSelector(t), SupportedLanguages.Default, StringComparison.OrdinalIgnoreCase));

        return fallback != null ? descriptionSelector(fallback) : null;
    }

    /// <summary>
    /// Resolve o código do idioma efetivamente utilizado (o solicitado ou o fallback padrão).
    /// </summary>
    /// <typeparam name="T">Tipo da entidade de tradução.</typeparam>
    /// <param name="translations">Coleção de traduções.</param>
    /// <param name="languageCode">Código do idioma desejado.</param>
    /// <param name="languageSelector">Função para extrair o LanguageCode da tradução.</param>
    /// <returns>Código do idioma utilizado ou null se não houver traduções.</returns>
    public static string? ResolveUsedLanguageCode<T>(
        IEnumerable<T>? translations,
        string languageCode,
        Func<T, string?> languageSelector)
    {
        if (translations == null || !translations.Any())
            return null;

        // Verifica se existe tradução no idioma solicitado
        var hasRequestedLanguage = translations.Any(t =>
            string.Equals(languageSelector(t), languageCode, StringComparison.OrdinalIgnoreCase));

        if (hasRequestedLanguage)
            return languageCode;

        // Fallback para o idioma padrão
        var hasDefaultLanguage = translations.Any(t =>
            string.Equals(languageSelector(t), SupportedLanguages.Default, StringComparison.OrdinalIgnoreCase));

        return hasDefaultLanguage ? SupportedLanguages.Default : null;
    }
}
