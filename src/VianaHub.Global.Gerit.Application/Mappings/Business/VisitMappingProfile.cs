using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Visit;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para Visit
/// </summary>
public class VisitMappingProfile : Profile
{
    public VisitMappingProfile()
    {
        CreateMap<VisitEntity, VisitResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

        CreateMap<ListPage<VisitEntity>, ListPageResponse<VisitResponse>>();
    }
}
