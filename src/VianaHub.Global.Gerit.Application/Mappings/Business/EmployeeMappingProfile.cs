using AutoMapper;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Employee;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<EmployeeEntity, EmployeeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.StatusDefinitionId, opt => opt.MapFrom(src => src.StatusDefinitionId))
            .ForMember(dest => dest.StatusDomainId, opt => opt.MapFrom(src => src.StatusDomainId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
            .ForMember(dest => dest.IsCellPhoneWhatsapp, opt => opt.MapFrom(src => src.IsCellPhoneWhatsapp))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<EmployeeEntity>, ListPageResponse<EmployeeResponse>>();
    }
}
