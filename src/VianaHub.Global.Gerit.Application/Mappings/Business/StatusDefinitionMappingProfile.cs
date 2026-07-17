using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDefinition;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Perfil de mapeamento AutoMapper para entidades StatusDefinition e DTOs.
/// </summary>
public class StatusDefinitionMappingProfile : Profile
{
    public StatusDefinitionMappingProfile()
    {
        CreateMap<StatusDefinitionEntity, StatusDefinitionResponse>()
            .ForMember(dest => dest.StatusDomainName, opt => opt.MapFrom(src =>
                src.StatusDomain != null
                    ? src.StatusDomain.Translations != null && src.StatusDomain.Translations.Any()
                        ? src.StatusDomain.Translations.First().Name ?? src.StatusDomain.Code
                        : src.StatusDomain.Code
                    : null));

        CreateMap<StatusDefinitionEntity, StatusDefinitionDetailResponse>()
            .IncludeBase<StatusDefinitionEntity, StatusDefinitionResponse>()
            .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

        CreateMap<StatusDefinitionTranslationsEntity, StatusDefinitionTranslationResponse>();

        CreateMap<ListPage<StatusDefinitionEntity>, ListPageResponse<StatusDefinitionResponse>>();
    }
}
