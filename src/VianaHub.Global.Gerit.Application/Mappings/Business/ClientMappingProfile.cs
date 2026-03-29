using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Profile de mapeamento para Client
/// </summary>
public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<ClientEntity, ClientResponse>()
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contacts.FirstOrDefault().Name));

        CreateMap<ClientEntity, ClientDetailResponse>()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => (int)src.Origin))
            .ForMember(dest => dest.OriginDescription, opt => opt.MapFrom(src => src.Origin.GetDescription()))
            .ForMember(dest => dest.ClientType, opt => opt.MapFrom(src => (int)src.ClientType))
            .ForMember(dest => dest.ClientTypeDescription, opt => opt.MapFrom(src => src.ClientType.GetDescription()));
        CreateMap<ListPage<ClientEntity>, ListPageResponse<ClientResponse>>();
    }
}
