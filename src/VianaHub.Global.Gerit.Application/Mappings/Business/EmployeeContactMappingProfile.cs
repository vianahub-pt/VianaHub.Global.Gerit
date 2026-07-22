using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class EmployeeContactPersonsMappingProfile : Profile
{
    public EmployeeContactPersonsMappingProfile()
    {
        CreateMap<EmployeeContactPersonsEntity, EmployeeContactResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee.Name))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
            .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
            .ForMember(dest => dest.IsCellPhoneWhatsapp, opt => opt.MapFrom(src => src.IsCellPhoneWhatsapp))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<EmployeeContactPersonsEntity, EmployeeContactDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee.Name))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
            .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
            .ForMember(dest => dest.IsCellPhoneWhatsapp, opt => opt.MapFrom(src => src.IsCellPhoneWhatsapp))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt));

        CreateMap<ListPage<EmployeeContactPersonsEntity>, ListPageResponse<EmployeeContactResponse>>();
    }
}
