using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Subscription;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para SubscriptionEntity
/// </summary>
public class SubscriptionMappingProfile : Profile
{
    public SubscriptionMappingProfile()
    {
        // Mapeia SubscriptionEntity -> SubscriptionResponse
        CreateMap<SubscriptionEntity, SubscriptionResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId))
            .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan != null ? src.Plan.Name : string.Empty))
            .ForMember(dest => dest.StripeId, opt => opt.MapFrom(src => src.StripeId))
            .ForMember(dest => dest.CurrentPeriodStart, opt => opt.MapFrom(src => src.CurrentPeriodStart))
            .ForMember(dest => dest.CurrentPeriodEnd, opt => opt.MapFrom(src => src.CurrentPeriodEnd))
            .ForMember(dest => dest.TrialStart, opt => opt.MapFrom(src => src.TrialStart))
            .ForMember(dest => dest.TrialEnd, opt => opt.MapFrom(src => src.TrialEnd))
            .ForMember(dest => dest.CancelAtPeriodEnd, opt => opt.MapFrom(src => src.CancelAtPeriodEnd))
            .ForMember(dest => dest.CanceledAt, opt => opt.MapFrom(src => src.CanceledAt))
            .ForMember(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.CancellationReason))
            .ForMember(dest => dest.StripeCustomerId, opt => opt.MapFrom(src => src.StripeCustomerId))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsTrial, opt => opt.MapFrom(src => 
                src.TrialEnd.HasValue && src.TrialEnd.Value > DateTime.UtcNow))
            .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => 
                (src.CurrentPeriodEnd - DateTime.UtcNow).Days > 0 
                    ? (src.CurrentPeriodEnd - DateTime.UtcNow).Days 
                    : 0));

        CreateMap<ListPage<SubscriptionEntity>, ListPageResponse<SubscriptionResponse>>();
    }
}
