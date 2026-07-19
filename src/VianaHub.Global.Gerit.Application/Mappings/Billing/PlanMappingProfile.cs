using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Plan;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

/// <summary>
/// Perfil de mapeamento do AutoMapper para SubscriptionPlanEntity.
/// Nota: Name e Description são resolvidos no PlanAppService a partir da tabela de traduções.
/// </summary>
public class PlanMappingProfile : Profile
{
    public PlanMappingProfile()
    {
        // Mapeia SubscriptionPlanEntity -> PlanResponse
        // Name e Description são preenchidos manualmente no PlanAppService.MapToResponse()
        // pois dependem da cultura do utilizador e da tabela de traduções.
        CreateMap<SubscriptionPlanEntity, PlanResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PricePerHour, opt => opt.MapFrom(src => src.PricePerHour))
            .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
            .ForMember(dest => dest.PricePerMonth, opt => opt.MapFrom(src => src.PricePerMonth))
            .ForMember(dest => dest.PricePerYear, opt => opt.MapFrom(src => src.PricePerYear))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.MaxUsers, opt => opt.MapFrom(src => src.MaxUsers))
            .ForMember(dest => dest.MaxPhotosPerVisit, opt => opt.MapFrom(src => src.MaxPhotosPerVisit))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore());

        CreateMap<ListPage<SubscriptionPlanEntity>, ListPageResponse<PlanResponse>>();
    }
}
