using VianaHub.Global.Gerit.Domain.Services;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Domain.Services;

public class TranslationResolverTests
{
    // Entidade de tradução simulada para os testes
    private sealed class TestTranslation
    {
        public string? LanguageCode { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
    }

    [Fact(DisplayName = "TranslationResolver.Resolve — retorna nome no idioma exato quando existe")]
    [Trait("Domain", "TranslationResolver")]
    public void Resolve_ReturnsExactMatch_WhenLanguageExists()
    {
        var translations = new List<TestTranslation>
        {
            new() { LanguageCode = "pt-PT", Name = "Nome PT" },
            new() { LanguageCode = "en-US", Name = "Name EN" },
            new() { LanguageCode = "es-ES", Name = "Nombre ES" },
        };

        var result = TranslationResolver.Resolve(translations, "en-US", t => t.LanguageCode, t => t.Name);

        Assert.Equal("Name EN", result);
    }

    [Fact(DisplayName = "TranslationResolver.Resolve — fallback para pt-PT quando idioma não encontrado")]
    [Trait("Domain", "TranslationResolver")]
    public void Resolve_ReturnsFallback_WhenLanguageNotFound()
    {
        var translations = new List<TestTranslation>
        {
            new() { LanguageCode = "pt-PT", Name = "Nome PT" },
            new() { LanguageCode = "es-ES", Name = "Nombre ES" },
        };

        var result = TranslationResolver.Resolve(translations, "fr-FR", t => t.LanguageCode, t => t.Name);

        Assert.Equal("Nome PT", result);
    }

    [Fact(DisplayName = "TranslationResolver.Resolve — retorna null quando coleção é nula")]
    [Trait("Domain", "TranslationResolver")]
    public void Resolve_ReturnsNull_WhenCollectionIsNull()
    {
        var result = TranslationResolver.Resolve<TestTranslation>(null, "pt-PT", t => t.LanguageCode, t => t.Name);

        Assert.Null(result);
    }

    [Fact(DisplayName = "TranslationResolver.Resolve — retorna null quando coleção está vazia")]
    [Trait("Domain", "TranslationResolver")]
    public void Resolve_ReturnsNull_WhenCollectionIsEmpty()
    {
        var translations = new List<TestTranslation>();

        var result = TranslationResolver.Resolve(translations, "pt-PT", t => t.LanguageCode, t => t.Name);

        Assert.Null(result);
    }

    [Fact(DisplayName = "TranslationResolver.Resolve — case insensitive no idioma")]
    [Trait("Domain", "TranslationResolver")]
    public void Resolve_IsCaseInsensitive_ForLanguageCode()
    {
        var translations = new List<TestTranslation>
        {
            new() { LanguageCode = "pt-PT", Name = "Nome PT" },
            new() { LanguageCode = "en-US", Name = "Name EN" },
        };

        var result = TranslationResolver.Resolve(translations, "EN-us", t => t.LanguageCode, t => t.Name);

        Assert.Equal("Name EN", result);
    }

    [Fact(DisplayName = "TranslationResolver.ResolveDescription — retorna descrição no idioma exato")]
    [Trait("Domain", "TranslationResolver")]
    public void ResolveDescription_ReturnsExactMatch_WhenLanguageExists()
    {
        var translations = new List<TestTranslation>
        {
            new() { LanguageCode = "pt-PT", Description = "Descrição PT" },
            new() { LanguageCode = "en-US", Description = "Description EN" },
        };

        var result = TranslationResolver.ResolveDescription(translations, "en-US", t => t.LanguageCode, t => t.Description);

        Assert.Equal("Description EN", result);
    }

    [Fact(DisplayName = "TranslationResolver.ResolveDescription — fallback para pt-PT quando idioma não encontrado")]
    [Trait("Domain", "TranslationResolver")]
    public void ResolveDescription_ReturnsFallback_WhenLanguageNotFound()
    {
        var translations = new List<TestTranslation>
        {
            new() { LanguageCode = "pt-PT", Description = "Descrição PT" },
            new() { LanguageCode = "es-ES", Description = "Descripción ES" },
        };

        var result = TranslationResolver.ResolveDescription(translations, "de-DE", t => t.LanguageCode, t => t.Description);

        Assert.Equal("Descrição PT", result);
    }

    [Fact(DisplayName = "TranslationResolver.ResolveDescription — retorna null quando coleção é nula")]
    [Trait("Domain", "TranslationResolver")]
    public void ResolveDescription_ReturnsNull_WhenCollectionIsNull()
    {
        var result = TranslationResolver.ResolveDescription<TestTranslation>(null, "pt-PT", t => t.LanguageCode, t => t.Description);

        Assert.Null(result);
    }
}
