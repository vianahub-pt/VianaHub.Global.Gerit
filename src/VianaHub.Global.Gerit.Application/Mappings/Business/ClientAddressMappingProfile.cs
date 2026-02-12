using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Perfil de mapeamento para ClientAddress
/// </summary>
public class ClientAddressMappingProfile : Profile
{
    public ClientAddressMappingProfile()
    {
        CreateMap<ClientAddressEntity, ClientAddressResponse>();
        
        CreateMap<ListPage<ClientAddressEntity>, ListPageResponse<ClientAddressResponse>>();
    }
}
