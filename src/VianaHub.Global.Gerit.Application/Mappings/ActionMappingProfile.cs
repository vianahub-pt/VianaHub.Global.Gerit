using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Action;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para ActionEntity
/// </summary>
public class ActionMappingProfile : Profile
{
    public ActionMappingProfile()
    {
        // Mapeia ActionEntity -> ActionResponse
        CreateMap<ActionEntity, ActionResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<ActionEntity>, ListPageResponse<ActionResponse>>();
    }
}
