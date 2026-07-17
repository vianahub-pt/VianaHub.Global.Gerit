using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Perfil de mapeamento AutoMapper para entidades DocumentType e DTOs.
/// </summary>
public class DocumentTypeMappingProfile : Profile
{
    public DocumentTypeMappingProfile()
    {
        CreateMap<DocumentTypeEntity, DocumentTypeResponse>();

        CreateMap<DocumentTypeEntity, DocumentTypeDetailResponse>()
            .IncludeBase<DocumentTypeEntity, DocumentTypeResponse>()
            .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

        CreateMap<DocumentTypeTranslationsEntity, DocumentTypeTranslationResponse>();

        CreateMap<ListPage<DocumentTypeEntity>, ListPageResponse<DocumentTypeResponse>>();
    }
}
