using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantAddress;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

public class TenantAddressMappingProfile : Profile
{
    public TenantAddressMappingProfile()
    {
        CreateMap<TenantAddressesEntity, TenantAddressResponse>();
        CreateMap<TenantAddressesEntity, TenantAddressDetailResponse>();
        CreateMap<ListPage<TenantAddressesEntity>, ListPageResponse<TenantAddressResponse>>();
    }
}
