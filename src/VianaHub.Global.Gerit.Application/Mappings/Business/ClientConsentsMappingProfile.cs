using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientConsentsMappingProfile : Profile
{
    public ClientConsentsMappingProfile()
    {
        CreateMap<ClientConsentsEntity, ClientConsentsResponse>();
        CreateMap<ListPage<ClientConsentsEntity>, ListPageResponse<ClientConsentsResponse>>();
    }
}
