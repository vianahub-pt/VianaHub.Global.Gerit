using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionTeamEquipments;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class InterventionTeamEquipmentMappingProfile : Profile
{
    public InterventionTeamEquipmentMappingProfile()
    {
        CreateMap<InterventionTeamEquipmentEntity, InterventionTeamEquipmentResponse>()
            .ForMember(dest => dest.InterventionTeam, opt => opt.MapFrom(src => src.InterventionTeam.Intervention.Title))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment.Name));
        CreateMap<ListPage<InterventionTeamEquipmentEntity>, ListPageResponse<InterventionTeamEquipmentResponse>>();
    }
}
