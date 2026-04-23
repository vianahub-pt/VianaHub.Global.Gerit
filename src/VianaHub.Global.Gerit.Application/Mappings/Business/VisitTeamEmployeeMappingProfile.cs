using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitTeamEmployee;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEmployee;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class VisitTeamEmployeeMappingProfile : Profile
{
    public VisitTeamEmployeeMappingProfile()
    {
        CreateMap<VisitTeamEmployeeEntity, VisitTeamEmployeeResponse>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name))
            .ForMember(dest => dest.FunctionName, opt => opt.MapFrom(src => src.Function.Name));

        CreateMap<CreateVisitTeamEmployeeRequest, VisitTeamEmployeeEntity>();
        CreateMap<UpdateVisitTeamEmployeeRequest, VisitTeamEmployeeEntity>();
    }
}
