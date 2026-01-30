using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Job;
using VianaHub.Global.Gerit.Domain.Entities.Job;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Job;

public class JobMappingProfile : Profile
{
    public JobMappingProfile()
    {
        CreateMap<JobDefinitionEntity, JobResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.JobCategory, opt => opt.MapFrom(src => src.JobCategory))
            .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.JobName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.JobPurpose, opt => opt.MapFrom(src => src.JobPurpose))
            .ForMember(dest => dest.JobType, opt => opt.MapFrom(src => src.JobType))
            .ForMember(dest => dest.JobMethod, opt => opt.MapFrom(src => src.JobMethod))
            .ForMember(dest => dest.CronExpression, opt => opt.MapFrom(src => src.CronExpression))
            .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId))
            .ForMember(dest => dest.ExecuteOnlyOnce, opt => opt.MapFrom(src => src.ExecuteOnlyOnce))
            .ForMember(dest => dest.TimeoutMinutes, opt => opt.MapFrom(src => src.TimeoutMinutes))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.Queue, opt => opt.MapFrom(src => src.Queue))
            .ForMember(dest => dest.MaxRetries, opt => opt.MapFrom(src => src.MaxRetries))
            .ForMember(dest => dest.JobConfiguration, opt => opt.MapFrom(src => src.JobConfiguration))
            .ForMember(dest => dest.IsSystemJob, opt => opt.MapFrom(src => src.IsSystemJob))
            .ForMember(dest => dest.HangfireJobId, opt => opt.MapFrom(src => src.HangfireJobId))
            .ForMember(dest => dest.LastRegisteredAt, opt => opt.MapFrom(src => src.LastRegisteredAt))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.ModifiedAt));

        CreateMap<ListPage<JobDefinitionEntity>, ListPageResponse<JobResponse>>();
    }
}
