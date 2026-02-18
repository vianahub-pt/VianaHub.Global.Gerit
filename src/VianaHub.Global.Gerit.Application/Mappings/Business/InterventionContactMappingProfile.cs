using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para InterventionContact
/// </summary>
public class InterventionContactMappingProfile : Profile
{
    public InterventionContactMappingProfile()
    {
        CreateMap<InterventionContactEntity, InterventionContactResponse>()
            .ForMember(dest => dest.Intervention, opt => opt.MapFrom(src => src.Intervention.Title)); 
        CreateMap<ListPage<InterventionContactEntity>, ListPageResponse<InterventionContactResponse>>();
    }
}
