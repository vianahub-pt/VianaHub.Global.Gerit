using AutoMapper;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Services;

namespace VianaHub.Global.Gerit.Application.Mappings.Resolvers;

/// <summary>
/// Resolve o nome traduzido de um StatusDomain com base no idioma da requisição.
/// </summary>
public class StatusDomainNameResolver : IValueResolver<StatusDomainEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public StatusDomainNameResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(StatusDomainEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.Resolve(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}
