using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientFiscalData;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientFiscalDataMappingProfile : Profile
{
    public ClientFiscalDataMappingProfile()
    {
        CreateMap<ClientFiscalDataEntity, ClientFiscalDataResponse>()
            .ForMember(dest => dest.ClientTypeId, opt => opt.MapFrom(src => (int)src.Client.ClientType))
            .ForMember(dest => dest.ClientType, opt => opt.MapFrom(src => src.Client.ClientType.GetDescription()))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => ResolveName(src.Client)));

        CreateMap<ClientFiscalDataEntity, ClientFiscalDataDetailResponse>()
            .ForMember(dest => dest.ClientTypeId, opt => opt.MapFrom(src => (int)src.Client.ClientType))
            .ForMember(dest => dest.ClientType, opt => opt.MapFrom(src => src.Client.ClientType.GetDescription()))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => ResolveName(src.Client)));

        CreateMap<ListPage<ClientFiscalDataEntity>, ListPageResponse<ClientFiscalDataResponse>>();
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
