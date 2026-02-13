using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionStatus;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para InterventionStatus
/// </summary>
public class InterventionStatusMappingProfile : Profile
{
    public InterventionStatusMappingProfile()
    {
        CreateMap<InterventionStatusEntity, InterventionStatusResponse>();
        CreateMap<ListPage<InterventionStatusEntity>, ListPageResponse<InterventionStatusResponse>>();
    }
}
