using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para VisitContact
/// </summary>
public class VisitContactMappingProfile : Profile
{
    public VisitContactMappingProfile()
    {
        CreateMap<VisitContactEntity, VisitContactResponse>()
            .ForMember(dest => dest.Visit, opt => opt.MapFrom(src => src.Visit.Title)); 
        CreateMap<ListPage<VisitContactEntity>, ListPageResponse<VisitContactResponse>>();
    }
}
