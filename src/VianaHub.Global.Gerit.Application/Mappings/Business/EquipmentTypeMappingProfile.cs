using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.EquipmentType;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class EquipmentTypeMappingProfile : Profile
{
    public EquipmentTypeMappingProfile()
    {
        CreateMap<EquipmentTypeEntity, EquipmentTypeResponse>();
        CreateMap<ListPage<EquipmentTypeEntity>, ListPageResponse<EquipmentTypeResponse>>();
    }
}
