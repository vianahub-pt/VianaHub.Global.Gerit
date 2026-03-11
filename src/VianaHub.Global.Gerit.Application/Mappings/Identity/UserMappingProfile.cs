using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Identity.User;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Identity;

/// <summary>
/// Perfil de mapeamento do AutoMapper para UserEntity
/// </summary>
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Mapeia UserEntity -> UserResponse
        CreateMap<UserEntity, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
            .ForMember(dest => dest.LastAccessAt, opt => opt.MapFrom(src => src.LastAccessAt))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<UserEntity>, ListPageResponse<UserResponse>>();

        // UserPreferences mapping
        CreateMap<Domain.Entities.Identity.UserPreferencesEntity, Application.Dtos.Response.Identity.UserPreferences.UserPreferencesResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Appearance, opt => opt.MapFrom(src => src.Appearance))
            .ForMember(dest => dest.Locale, opt => opt.MapFrom(src => src.Locale))
            .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => src.Timezone))
            .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.DateFormat))
            .ForMember(dest => dest.TimeFormat, opt => opt.MapFrom(src => src.TimeFormat))
            .ForMember(dest => dest.DayStart, opt => opt.MapFrom(src => src.DayStart.ToString(@"hh\:mm")))
            .ForMember(dest => dest.EmailNewsletter, opt => opt.MapFrom(src => src.EmailNewsletter))
            .ForMember(dest => dest.EmailWeeklyReport, opt => opt.MapFrom(src => src.EmailWeeklyReport))
            .ForMember(dest => dest.EmailApproval, opt => opt.MapFrom(src => src.EmailApproval))
            .ForMember(dest => dest.EmailAlerts, opt => opt.MapFrom(src => src.EmailAlerts))
            .ForMember(dest => dest.EmailReminders, opt => opt.MapFrom(src => src.EmailReminders))
            .ForMember(dest => dest.EmailPlanner, opt => opt.MapFrom(src => src.EmailPlanner))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
    }
}
