using AutoMapper;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Billing.TenantAddress;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Billing;

public class TenantAddressMappingProfile : Profile
{
    public TenantAddressMappingProfile()
    {
        CreateMap<TenantAddressesEntity, TenantAddressResponse>()
            .ForMember(dest => dest.AddressTypeName, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.AddressType?.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)));

        CreateMap<TenantAddressesEntity, TenantAddressDetailResponse>()
            .ForMember(dest => dest.AddressTypeName, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.AddressType?.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)));

        CreateMap<ListPage<TenantAddressesEntity>, ListPageResponse<TenantAddressResponse>>();
    }
}
