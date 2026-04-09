using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompany;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientCompanyMappingProfile : Profile
{
    public ClientCompanyMappingProfile()
    {
        CreateMap<ClientCompanyEntity, ClientCompanyResponse>();
        CreateMap<ListPage<ClientCompanyEntity>, ListPageResponse<ClientCompanyResponse>>();
    }
}
