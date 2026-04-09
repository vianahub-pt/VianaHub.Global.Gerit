using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividualFiscalData;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientIndividualFiscalDataMappingProfile : Profile
{
    public ClientIndividualFiscalDataMappingProfile()
    {
        CreateMap<ClientIndividualFiscalDataEntity, ClientIndividualFiscalDataResponse>();
        CreateMap<ListPage<ClientIndividualFiscalDataEntity>, ListPageResponse<ClientIndividualFiscalDataResponse>>();
    }
}
