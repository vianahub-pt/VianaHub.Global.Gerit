using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Plan;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para PlanEntity
/// </summary>
public class PlanMappingProfile : Profile
{
    public PlanMappingProfile()
    {
        // Mapeia PlanEntity -> PlanResponse
        CreateMap<PlanEntity, PlanResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PricePerHour, opt => opt.MapFrom(src => src.PricePerHour))
            .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
            .ForMember(dest => dest.PricePerMonth, opt => opt.MapFrom(src => src.PricePerMonth))
            .ForMember(dest => dest.PricePerYear, opt => opt.MapFrom(src => src.PricePerYear))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.MaxUsers, opt => opt.MapFrom(src => src.MaxUsers))
            .ForMember(dest => dest.MaxPhotosPerInterventions, opt => opt.MapFrom(src => src.MaxPhotosPerInterventions))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<PlanEntity>, ListPageResponse<PlanResponse>>();
    }
}
