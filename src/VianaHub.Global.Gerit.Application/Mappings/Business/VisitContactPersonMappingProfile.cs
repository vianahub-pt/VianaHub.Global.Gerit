using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContactPersons;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para VisitContact
/// </summary>
public class VisitContactPersonsMappingProfile : Profile
{
    public VisitContactPersonsMappingProfile()
    {
        CreateMap<VisitContactPersonsEntity, VisitContactPersonResponse>();
        CreateMap<ListPage<VisitContactPersonsEntity>, ListPageResponse<VisitContactPersonResponse>>();
    }
}
