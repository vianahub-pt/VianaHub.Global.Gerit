namespace VianaHub.Global.Gerit.Application.Mappings.Business;

using AutoMapper;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDomain;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

/// <summary>
/// Perfil de mapeamento AutoMapper para entidades StatusDomain e DTOs.
/// </summary>
public class StatusDomainMappingProfile : Profile
{
    public StatusDomainMappingProfile()
    {
        CreateMap<StatusDomainEntity, StatusDomainResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<StatusDomainEntity, StatusDomainDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveDescription(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Description)))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<StatusDomainEntity>, ListPageResponse<StatusDomainResponse>>();
    }
}
