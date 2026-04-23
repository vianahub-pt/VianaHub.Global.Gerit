using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.OriginType;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class OriginTypeMappingProfile : Profile
{
    public OriginTypeMappingProfile()
    {
        CreateMap<OriginTypeEntity, OriginTypeResponse>();
        CreateMap<ListPage<OriginTypeEntity>, ListPageResponse<OriginTypeResponse>>();
    }
}
