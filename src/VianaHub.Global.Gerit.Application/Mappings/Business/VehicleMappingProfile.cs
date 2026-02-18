using AutoMapper;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Vehicle;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<VehicleEntity, VehicleResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

        CreateMap<ListPage<VehicleEntity>, ListPageResponse<VehicleResponse>>();
    }
}
