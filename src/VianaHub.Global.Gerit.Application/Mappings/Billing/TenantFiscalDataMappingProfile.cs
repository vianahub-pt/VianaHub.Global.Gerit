using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantFiscalData;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

public class TenantFiscalDataMappingProfile : Profile
{
    public TenantFiscalDataMappingProfile()
    {
        CreateMap<TenantFiscalDataEntity, TenantFiscalDataResponse>();
        CreateMap<TenantFiscalDataEntity, TenantFiscalDataDetailResponse>();
        CreateMap<ListPage<TenantFiscalDataEntity>, ListPageResponse<TenantFiscalDataResponse>>();
    }
}
