using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para VisitContact
/// </summary>
public class VisitContactPersonsMappingProfile : Profile
{
    public VisitContactPersonsMappingProfile()
    {
        CreateMap<VisitContactPersonsEntity, VisitContactResponse>()
            .ForMember(dest => dest.Visit, opt => opt.MapFrom(src => src.Visit.Title)); 
        CreateMap<ListPage<VisitContactPersonsEntity>, ListPageResponse<VisitContactResponse>>();
    }
}
