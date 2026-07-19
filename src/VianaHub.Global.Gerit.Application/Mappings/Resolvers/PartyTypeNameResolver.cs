using AutoMapper;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Services;

namespace VianaHub.Global.Gerit.Application.Mappings.Resolvers;

/// <summary>
/// Resolve o nome traduzido de um PartyType com base no idioma da requisição.
/// </summary>
public class PartyTypeNameResolver : IValueResolver<PartyTypeEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public PartyTypeNameResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(PartyTypeEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.Resolve(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}

/// <summary>
/// Resolve o nome traduzido de um PartyType para uso em DTOs que contêm PartyType como propriedade.
/// </summary>
public class PartyTypeNameValueResolver : IValueResolver<ClientEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public PartyTypeNameValueResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(ClientEntity source, object destination, string? destMember, ResolutionContext context)
    {
        var partyType = source.PartyType;
        if (partyType?.Translations == null)
            return null;

        return TranslationResolver.Resolve(
            partyType.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}
