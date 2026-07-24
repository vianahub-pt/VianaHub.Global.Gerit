using AutoMapper;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Perfil de mapeamento AutoMapper para entidades DocumentType e DTOs.
/// </summary>
public class DocumentTypeMappingProfile : Profile
{
    public DocumentTypeMappingProfile()
    {
        CreateMap<DocumentTypeEntity, DocumentTypeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<DocumentTypeEntity, DocumentTypeDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveDescription(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Description)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<DocumentTypeTranslationsEntity, DocumentTypeTranslationResponse>();

        CreateMap<ListPage<DocumentTypeEntity>, ListPageResponse<DocumentTypeResponse>>();
    }
}
