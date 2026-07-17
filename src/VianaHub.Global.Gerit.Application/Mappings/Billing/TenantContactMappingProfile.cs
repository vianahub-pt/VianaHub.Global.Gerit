using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContact;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

public class TenantContactPersonsMappingProfile : Profile
{
    public TenantContactPersonsMappingProfile()
    {
        CreateMap<TenantContactPersonsEntity, TenantContactResponse>();
        CreateMap<ListPage<TenantContactPersonsEntity>, ListPageResponse<TenantContactResponse>>();
    }
}
