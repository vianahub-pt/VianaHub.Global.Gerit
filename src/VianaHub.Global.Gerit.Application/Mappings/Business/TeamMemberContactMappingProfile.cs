using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class TeamMemberContactMappingProfile : Profile
{
    public TeamMemberContactMappingProfile()
    {
        CreateMap<TeamMemberContactEntity, TeamMemberContactResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.TeamMemberId, opt => opt.MapFrom(src => src.TeamMemberId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<TeamMemberContactEntity>, ListPageResponse<TeamMemberContactResponse>>();
    }
}
