using AutoMapper;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.FileType;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class FileTypeMappingProfile : Profile
{
    public FileTypeMappingProfile()
    {
        CreateMap<FileTypeEntity, FileTypeResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => src.Extension))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<ListPage<FileTypeEntity>, ListPageResponse<FileTypeResponse>>();
    }
}
