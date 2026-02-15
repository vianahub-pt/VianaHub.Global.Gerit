using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusType;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para StatusType
/// </summary>
public class StatusTypeMappingProfile : Profile
{
    public StatusTypeMappingProfile()
    {
        CreateMap<StatusTypeEntity, StatusTypeResponse>();
        CreateMap<ListPage<StatusTypeEntity>, ListPageResponse<StatusTypeResponse>>();
    }
}
