using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientConsentsMappingProfile : Profile
{
    public ClientConsentsMappingProfile()
    {
        CreateMap<ClientConsentsEntity, ClientConsentResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => ResolveName(src.Client)))
            .ForMember(dest => dest.ConsentType, opt => opt.MapFrom(src => src.ConsentType.Name))
            .ForMember(dest => dest.ConsentOriginType, opt => opt.MapFrom(src => src.ConsentOriginType.Name));
        
        CreateMap<ClientConsentsEntity, ClientConsentDetailResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => ResolveName(src.Client)))
            .ForMember(dest => dest.ConsentType, opt => opt.MapFrom(src => src.ConsentType.Name))
            .ForMember(dest => dest.ConsentOriginType, opt => opt.MapFrom(src => src.ConsentOriginType.Name));

        CreateMap<ListPage<ClientConsentsEntity>, ListPageResponse<ClientConsentResponse>>();
    }

    private static string? ResolveName(ClientEntity src)
    {
        return src.ClientType switch
        {
            ClientType.PessoaSingular => src.Individual?.FullName,
            ClientType.RecibosVerdes => src.Individual?.FullName,
            ClientType.Freelancer => src.Individual?.FullName,
            ClientType.PessoaJuridica => src.Company?.DisplayName,
            ClientType.SociedadeUnipessoalQuotas => src.Company?.DisplayName,
            _ => null
        };
    }
}
