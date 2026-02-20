using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeams;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class InterventionTeamMappingProfile : Profile
{
    public InterventionTeamMappingProfile()
    {
        CreateMap<InterventionTeamEntity, InterventionTeamResponse>()
            .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.Intervention, opt => opt.MapFrom(src => src.Intervention.Title));

        CreateMap<ListPage<InterventionTeamEntity>, ListPageResponse<InterventionTeamResponse>>();
    }
}
