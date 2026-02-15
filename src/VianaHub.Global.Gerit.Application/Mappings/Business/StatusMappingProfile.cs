using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Status;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para Status
/// </summary>
public class StatusMappingProfile : Profile
{
    public StatusMappingProfile()
    {
        CreateMap<StatusEntity, StatusResponse>();
        CreateMap<ListPage<StatusEntity>, ListPageResponse<StatusResponse>>();
    }
}
