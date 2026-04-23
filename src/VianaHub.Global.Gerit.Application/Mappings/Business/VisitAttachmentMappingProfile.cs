using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAttachment;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class VisitAttachmentMappingProfile : Profile
{
    public VisitAttachmentMappingProfile()
    {
        CreateMap<VisitAttachmentEntity, VisitAttachmentResponse>()
            .ForMember(dest => dest.FileTypeName, opt => opt.MapFrom(src => src.FileType.Extension))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.FileType.MimeType))
            .ForMember(dest => dest.AttachmentCategoryName, opt => opt.MapFrom(src => src.AttachmentCategory.Name))
            .ForMember(dest => dest.FormattedFileSize, opt => opt.MapFrom(src => src.GetFormattedFileSize()));

        CreateMap<CreateVisitAttachmentRequest, VisitAttachmentEntity>();
        CreateMap<UpdateVisitAttachmentRequest, VisitAttachmentEntity>();
    }
}
