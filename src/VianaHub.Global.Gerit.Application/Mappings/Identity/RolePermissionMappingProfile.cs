using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.RolePermission;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Application.Mappings.Identity;

public class RolePermissionMappingProfile : Profile
{
    public RolePermissionMappingProfile()
    {
        CreateMap<RolePermissionEntity, RolePermissionResponse>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty))
            .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource != null ? src.Resource.Name : string.Empty))
            .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.Action != null ? src.Action.Name : string.Empty));
    }
}
