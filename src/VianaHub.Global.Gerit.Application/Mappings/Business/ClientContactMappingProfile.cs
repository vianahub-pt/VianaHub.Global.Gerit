using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientContact;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
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
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => ResolveName(src.Client)));

        CreateMap<ClientContactEntity, ClientContactDetailResponse>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => ResolveName(src.Client)));

        CreateMap<ListPage<ClientContactEntity>, ListPageResponse<ClientContactResponse>>();
    }

    private static string? ResolveName(ClientEntity src)
    {
        return src.ClientType switch
        {
            ClientType.PessoaSingular => src.Individual?.DisplayName,
            ClientType.RecibosVerdes => src.Individual?.DisplayName,
            ClientType.Freelancer => src.Individual?.DisplayName,
            ClientType.PessoaJuridica => src.Company?.DisplayName,
            ClientType.SociedadeUnipessoalQuotas => src.Company?.DisplayName,
            _ => null
        };
    }
}
