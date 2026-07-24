using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContactPerson;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

public class TenantContactPersonsMappingProfile : Profile
{
    public TenantContactPersonsMappingProfile()
    {
        CreateMap<TenantContactPersonsEntity, TenantContactPersonResponse>();
        CreateMap<TenantContactPersonsEntity, TenantContactPersonDetailResponse>();
        CreateMap<ListPage<TenantContactPersonsEntity>, ListPageResponse<TenantContactPersonResponse>>();
    }
}
