using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeams;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class VisitTeamMappingProfile : Profile
{
    public VisitTeamMappingProfile()
    {
        CreateMap<VisitTeamEntity, VisitTeamResponse>()
            .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.Visit, opt => opt.MapFrom(src => src.Visit.Title));

        CreateMap<ListPage<VisitTeamEntity>, ListPageResponse<VisitTeamResponse>>();
    }
}
