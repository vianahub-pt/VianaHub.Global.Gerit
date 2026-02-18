using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMembersTeams;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class TeamMembersTeamMappingProfile : Profile
{
    public TeamMembersTeamMappingProfile()
    {
        CreateMap<TeamMembersTeamEntity, TeamMembersTeamResponse>()
            .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.TeamMember, opt => opt.MapFrom(src => src.TeamMember.Name));
        CreateMap<ListPage<TeamMembersTeamEntity>, ListPageResponse<TeamMembersTeamResponse>>();
    }
}
