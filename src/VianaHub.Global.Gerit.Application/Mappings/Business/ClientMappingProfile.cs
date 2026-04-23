using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.Client;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Helpers;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<ClientEntity, ClientResponse>()
            .ForMember(dest => dest.ClientType, opt => opt.MapFrom(src => (int)src.ClientType))
            .ForMember(dest => dest.ClientTypeDescription, opt => opt.MapFrom(src => src.ClientType.GetDescription()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => ResolveName(src)))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => ResolvePhoneNumber(src)))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => ResolveEmail(src)))
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => ResolvePrimaryContact(src)));

        CreateMap<ClientEntity, ClientDetailResponse>()
            .ForMember(dest => dest.ClientType, opt => opt.MapFrom(src => (int)src.ClientType))
            .ForMember(dest => dest.ClientTypeDescription, opt => opt.MapFrom(src => src.ClientType.GetDescription()))
            .ForMember(dest => dest.OriginType, opt => opt.MapFrom(src => (int)src.OriginType))
            .ForMember(dest => dest.OriginTypeDescription, opt => opt.MapFrom(src => src.OriginType.GetDescription()))
            .ForMember(dest => dest.UrlImage, opt => opt.MapFrom(src => src.UrlImage))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));

        CreateMap<ClientIndividualEntity, ClientIndividualDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
            .ForMember(dest => dest.IsWhatsapp, opt => opt.MapFrom(src => src.IsWhatsapp))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
            .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ClientCompanyEntity, ClientCompanyDetailResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId))
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
            .ForMember(dest => dest.LegalName, opt => opt.MapFrom(src => src.LegalName))
            .ForMember(dest => dest.TradeName, opt => opt.MapFrom(src => src.TradeName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
            .ForMember(dest => dest.IsWhatsapp, opt => opt.MapFrom(src => src.IsWhatsapp))
            .ForMember(dest => dest.CompanyRegistration, opt => opt.MapFrom(src => src.CompanyRegistration))
            .ForMember(dest => dest.CAE, opt => opt.MapFrom(src => src.CAE))
            .ForMember(dest => dest.NumberOfEmployee, opt => opt.MapFrom(src => src.NumberOfEmployee))
            .ForMember(dest => dest.LegalRepresentative, opt => opt.MapFrom(src => src.LegalRepresentative))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<ClientEntity>, ListPageResponse<ClientResponse>>();
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

    private static string? ResolvePhoneNumber(ClientEntity src)
    {
        return src.ClientType switch
        {
            ClientType.PessoaSingular => src.Individual?.PhoneNumber,
            ClientType.RecibosVerdes => src.Individual?.PhoneNumber,
            ClientType.Freelancer => src.Individual?.PhoneNumber,
            ClientType.PessoaJuridica => src.Company?.PhoneNumber,
            ClientType.SociedadeUnipessoalQuotas => src.Company?.PhoneNumber,
            _ => null
        };
    }

    private static string? ResolveEmail(ClientEntity src)
    {
        return src.ClientType switch
        {
            ClientType.PessoaSingular => src.Individual?.Email,
            ClientType.RecibosVerdes => src.Individual?.Email,
            ClientType.Freelancer => src.Individual?.Email,
            ClientType.PessoaJuridica => src.Company?.Email,
            ClientType.SociedadeUnipessoalQuotas => src.Company?.Email,
            _ => null
        };
    }

    private static string? ResolvePrimaryContact(ClientEntity src)
    {
        return src.Contacts
            .FirstOrDefault(x => x.IsPrimary && !x.IsDeleted)?.Name
            ?? src.Contacts.FirstOrDefault(x => !x.IsDeleted)?.Name;
    }
}