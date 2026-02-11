using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para Client
/// </summary>
public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<ClientEntity, ClientResponse>();
        CreateMap<ListPage<ClientEntity>, ListPageResponse<ClientResponse>>();
    }
}
