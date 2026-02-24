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
        CreateMap<InterventionTeamVehicleEntity, InterventionTeamVehicleResponse>();
        CreateMap<ListPage<InterventionTeamVehicleEntity>, ListPageResponse<InterventionTeamVehicleResponse>>();
    }
}
