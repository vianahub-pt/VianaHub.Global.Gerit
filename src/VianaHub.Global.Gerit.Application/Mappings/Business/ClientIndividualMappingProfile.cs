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
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Individual.DisplayName));

        CreateMap<CreateClientIndividualRequest, ClientIndividualEntity>();
        CreateMap<UpdateClientIndividualRequest, ClientIndividualEntity>();
    }
}
