using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para ClientContact
/// </summary>
public class ClientContactMappingProfile : Profile
{
    public ClientContactMappingProfile()
    {
        CreateMap<ClientContactEntity, ClientContactResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.Individual.DisplayName));
        CreateMap<ListPage<ClientContactEntity>, ListPageResponse<ClientContactResponse>>();
    }
}
