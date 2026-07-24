using AutoMapper;
using System.Globalization;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Services;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

/// <summary>
/// Perfil de mapeamento para ClientAddress
/// </summary>
public class ClientAddressesMappingProfile : Profile
{
    public ClientAddressesMappingProfile()
    {
        CreateMap<ClientAddressesEntity, ClientAddressResponse>()
            .ForMember(dest => dest.AddressTypeName, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.AddressType.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)));

        CreateMap<ListPage<ClientAddressesEntity>, ListPageResponse<ClientAddressResponse>>();
    }
}
