using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Resource;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper para ResourceEntity
/// </summary>
public class ResourceMappingProfile : Profile
{
    public ResourceMappingProfile()
    {
        // Mapeia ResourceEntity -> ResourceResponse
        CreateMap<ResourceEntity, ResourceResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<ResourceEntity>, ListPageResponse<ResourceResponse>>();
    }
}
