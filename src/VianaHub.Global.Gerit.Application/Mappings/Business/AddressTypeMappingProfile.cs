using AutoMapper;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.AddressType;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class AddressTypeMappingProfile : Profile
{
    public AddressTypeMappingProfile()
    {
        CreateMap<AddressTypeEntity, AddressTypeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<AddressTypeEntity>, ListPageResponse<AddressTypeResponse>>();
    }
}
