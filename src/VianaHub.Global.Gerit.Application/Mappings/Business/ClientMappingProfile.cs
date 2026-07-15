using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<ClientEntity, ClientResponse>()
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => ResolvePrimaryContact(src)));

        CreateMap<ClientEntity, ClientDetailResponse>();

        CreateMap<ListPage<ClientEntity>, ListPageResponse<ClientResponse>>();
    }

    private static string? ResolvePrimaryContact(ClientEntity src)
    {
        return src.Contacts
            .FirstOrDefault(x => x.IsPrimary && !x.IsDeleted)?.Name
            ?? src.Contacts.FirstOrDefault(x => !x.IsDeleted)?.Name;
    }
}
