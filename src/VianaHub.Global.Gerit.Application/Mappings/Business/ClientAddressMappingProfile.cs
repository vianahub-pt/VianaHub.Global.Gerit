using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Perfil de mapeamento para ClientAddress
/// </summary>
public class ClientAddressesMappingProfile : Profile
{
    public ClientAddressesMappingProfile()
    {
        CreateMap<ClientAddressesEntity, ClientAddressResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType.Name));
        
        CreateMap<ListPage<ClientAddressesEntity>, ListPageResponse<ClientAddressResponse>>();
    }
}
