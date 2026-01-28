using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.UserRole;
using VianaHub.Global.Gerit.Domain.Entities;

namespace VianaHub.Global.Gerit.Application.Mappings.Identity;

public class UserRoleMappingProfile : Profile
{
    public UserRoleMappingProfile()
    {
        CreateMap<UserRoleEntity, UserRoleResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));
    }
}
