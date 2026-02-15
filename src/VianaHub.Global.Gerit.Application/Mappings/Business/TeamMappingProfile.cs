using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Team;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>();
        CreateMap<ListPage<TeamEntity>, ListPageResponse<TeamResponse>>();
    }
}
