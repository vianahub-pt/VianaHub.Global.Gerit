using AutoMapper;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Services;

namespace VianaHub.Global.Gerit.Application.Mappings.Resolvers;

/// <summary>
/// Resolve o nome traduzido de um AcquisitionSourceType com base no idioma da requisição.
/// </summary>
public class AcquisitionSourceTypeNameResolver : IValueResolver<AcquisitionSourceTypeEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public AcquisitionSourceTypeNameResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(AcquisitionSourceTypeEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.Resolve(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}

/// <summary>
/// Resolve o nome traduzido de AcquisitionSourceType para ClientEntity.Base (para uso em mapeamentos de Client).
/// </summary>
public class ClientAcquisitionSourceTypeNameResolver : IValueResolver<ClientEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public ClientAcquisitionSourceTypeNameResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(ClientEntity source, object destination, string? destMember, ResolutionContext context)
    {
        var acquisitionSourceType = source.AcquisitionSourceType;
        if (acquisitionSourceType?.Translations == null)
            return null;

        return TranslationResolver.Resolve(
            acquisitionSourceType.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}
