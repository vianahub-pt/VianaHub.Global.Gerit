using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientCompanyFiscalData;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientCompanyFiscalDataMappingProfile : Profile
{
    public ClientCompanyFiscalDataMappingProfile()
    {
        CreateMap<ClientCompanyFiscalDataEntity, ClientCompanyFiscalDataResponse>();
        CreateMap<ListPage<ClientCompanyFiscalDataEntity>, ListPageResponse<ClientCompanyFiscalDataResponse>>();
    }
}
