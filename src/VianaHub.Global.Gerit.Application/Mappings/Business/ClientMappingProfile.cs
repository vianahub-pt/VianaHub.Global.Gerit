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
            .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => ResolveTenant(src)))
            .ForMember(dest => dest.PartyType, opt => opt.MapFrom(src => ResolvePartyType(src)))
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => ResolvePrimaryContact(src)));

        CreateMap<ClientEntity, ClientDetailResponse>()
            .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => ResolveTenant(src)))
            .ForMember(dest => dest.PartyType, opt => opt.MapFrom(src => ResolvePartyType(src)))
            .ForMember(dest => dest.AcquisitionSourceType, opt => opt.MapFrom(src => ResolveAcquisitionSourceType(src)));

        CreateMap<ListPage<ClientEntity>, ListPageResponse<ClientResponse>>();
    }

    private static string? ResolvePrimaryContact(ClientEntity src)
    {
        return src.Contacts
            .FirstOrDefault(x => x.IsPrimary && !x.IsDeleted)?.Name
            ?? src.Contacts.FirstOrDefault(x => !x.IsDeleted)?.Name;
    }

    private static string? ResolveTenant(ClientEntity src)
    {
        return src.Tenant?.Name;
    }
    private static string? ResolvePartyType(ClientEntity src)
    {
        return src.PartyType.Translations.FirstOrDefault()?.Name;
    }
    private static string? ResolveAcquisitionSourceType(ClientEntity src)
    {
        return src.AcquisitionSourceType.Translations.FirstOrDefault()?.Name;
    }
}
