using AutoMapper;
using System.Globalization;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.FileType;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Services;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class FileTypeMappingProfile : Profile
{
    public FileTypeMappingProfile()
    {
        CreateMap<FileTypeEntity, FileTypeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => src.Extension))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveUsedLanguageCode(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.Resolve(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Name)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, _, _) =>
                TranslationResolver.ResolveDescription(src.Translations, CultureInfo.CurrentCulture.Name, t => t.LanguageCode, t => t.Description)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<FileTypeEntity>, ListPageResponse<FileTypeResponse>>();
    }
}
