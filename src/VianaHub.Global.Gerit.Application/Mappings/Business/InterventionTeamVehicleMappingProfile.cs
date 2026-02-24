using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamVehicles;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class InterventionTeamVehicleMappingProfile : Profile
{
    public InterventionTeamVehicleMappingProfile()
    {
        CreateMap<InterventionTeamVehicleEntity, InterventionTeamVehicleResponse>()
            .ForMember(dest => dest.InterventionTeam, opt => opt.MapFrom(src => src.InterventionTeam.Intervention.Title))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle.Plate));
        CreateMap<ListPage<InterventionTeamVehicleEntity>, ListPageResponse<InterventionTeamVehicleResponse>>();
    }
}
