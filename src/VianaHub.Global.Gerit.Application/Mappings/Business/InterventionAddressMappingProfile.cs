using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionAddress;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class InterventionAddressMappingProfile : Profile
{
    public InterventionAddressMappingProfile()
    {
        CreateMap<InterventionAddressEntity, InterventionAddressResponse>()
            .ForMember(dest => dest.Intervention, opt => opt.MapFrom(src => src.Intervention.Title)); 
        CreateMap<ListPage<InterventionAddressEntity>, ListPageResponse<InterventionAddressResponse>>();
    }
}
