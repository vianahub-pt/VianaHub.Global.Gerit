using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividual;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividual;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientIndividualMappingProfile : Profile
{
    public ClientIndividualMappingProfile()
    {
        CreateMap<ClientIndividualEntity, ClientIndividualResponse>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.GetFullName()))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => 
                src.BirthDate.HasValue 
                    ? DateTime.UtcNow.Year - src.BirthDate.Value.Year - (DateTime.UtcNow.DayOfYear < src.BirthDate.Value.DayOfYear ? 1 : 0)
                    : (int?)null));

        CreateMap<CreateClientIndividualRequest, ClientIndividualEntity>();
        CreateMap<UpdateClientIndividualRequest, ClientIndividualEntity>();
    }
}
