using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientHierarchy;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientHierarchyMappingProfile : Profile
{
    public ClientHierarchyMappingProfile()
    {
        CreateMap<ClientHierarchyEntity, ClientHierarchyResponse>();
        CreateMap<ListPage<ClientHierarchyEntity>, ListPageResponse<ClientHierarchyResponse>>();
    }
}
