using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAddress;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class VisitAddressesMappingProfile : Profile
{
    public VisitAddressesMappingProfile()
    {
        CreateMap<VisitAddressesEntity, VisitAddressResponse>()
            .ForMember(dest => dest.Visit, opt => opt.MapFrom(src => src.Visit.Title)); 
        CreateMap<ListPage<VisitAddressesEntity>, ListPageResponse<VisitAddressResponse>>();
    }
}
