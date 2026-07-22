using AutoMapper;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDefinition;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Services;
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
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.StatusDomainName, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.StatusDomain?.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.Name : null))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(
                    src.Translations,
                    CultureInfo.CurrentCulture.Name,
                    t => t.LanguageCode)));

        CreateMap<StatusDefinitionEntity, StatusDefinitionDetailResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.StatusDomainName, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.StatusDomain?.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.Name : null))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(
                    src.Translations,
                    CultureInfo.CurrentCulture.Name,
                    t => t.LanguageCode)))
            .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

        CreateMap<StatusDefinitionTranslationsEntity, StatusDefinitionTranslationResponse>();

        CreateMap<ListPage<StatusDefinitionEntity>, ListPageResponse<StatusDefinitionResponse>>();
    }
}
