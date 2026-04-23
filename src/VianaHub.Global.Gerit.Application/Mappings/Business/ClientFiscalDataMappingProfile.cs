using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientFiscalData;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientFiscalDataMappingProfile : Profile
{
    public ClientFiscalDataMappingProfile()
    {
        CreateMap<ClientFiscalDataEntity, ClientFiscalDataResponse>();
        CreateMap<ListPage<ClientFiscalDataEntity>, ListPageResponse<ClientFiscalDataResponse>>();
    }
}
