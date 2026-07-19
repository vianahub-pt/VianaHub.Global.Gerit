using AutoMapper;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;
using VianaHub.Global.Gerit.Domain.Services;

namespace VianaHub.Global.Gerit.Application.Mappings.Resolvers;

/// <summary>
/// Resolve o nome traduzido de um DocumentType com base no idioma da requisição.
/// </summary>
public class DocumentTypeNameResolver : IValueResolver<DocumentTypeEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public DocumentTypeNameResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(DocumentTypeEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.Resolve(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Name);
    }
}

/// <summary>
/// Resolve a descrição traduzida de um DocumentType com base no idioma da requisição.
/// </summary>
public class DocumentTypeDescriptionResolver : IValueResolver<DocumentTypeEntity, object, string?>
{
    private readonly IRequestLanguageContext _languageContext;

    public DocumentTypeDescriptionResolver(IRequestLanguageContext languageContext)
    {
        _languageContext = languageContext;
    }

    public string? Resolve(DocumentTypeEntity source, object destination, string? destMember, ResolutionContext context)
    {
        return TranslationResolver.ResolveDescription(
            source.Translations,
            _languageContext.LanguageCode,
            t => t.LanguageCode,
            t => t.Description);
    }
}
