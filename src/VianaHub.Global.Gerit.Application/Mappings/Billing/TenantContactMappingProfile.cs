using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantContact;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

public class TenantContactMappingProfile : Profile
{
    public TenantContactMappingProfile()
    {
        CreateMap<TenantContactEntity, TenantContactResponse>();
        CreateMap<ListPage<TenantContactEntity>, ListPageResponse<TenantContactResponse>>();
    }
}
