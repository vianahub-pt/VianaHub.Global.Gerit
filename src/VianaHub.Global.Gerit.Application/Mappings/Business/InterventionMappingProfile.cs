using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Intervention;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para Intervention
/// </summary>
public class InterventionMappingProfile : Profile
{
    public InterventionMappingProfile()
    {
        CreateMap<InterventionEntity, InterventionResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.TeamMember, opt => opt.MapFrom(src => src.TeamMember.Name))
            .ForMember(dest => dest.Plate, opt => opt.MapFrom(src => src.Vehicle.Plate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

        CreateMap<ListPage<InterventionEntity>, ListPageResponse<InterventionResponse>>();
    }
}
