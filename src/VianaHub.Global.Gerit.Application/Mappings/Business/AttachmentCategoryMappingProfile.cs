using AutoMapper;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AttachmentCategory;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.AttachmentCategory;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Application.Mappings.Business;

public class AttachmentCategoryMappingProfile : Profile
{
    public AttachmentCategoryMappingProfile()
    {
        CreateMap<AttachmentCategoryEntity, AttachmentCategoryResponse>();
        CreateMap<CreateAttachmentCategoryRequest, AttachmentCategoryEntity>();
        CreateMap<UpdateAttachmentCategoryRequest, AttachmentCategoryEntity>();
    }
}
