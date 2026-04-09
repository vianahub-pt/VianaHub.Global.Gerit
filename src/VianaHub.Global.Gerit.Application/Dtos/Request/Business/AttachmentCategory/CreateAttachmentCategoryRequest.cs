namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.AttachmentCategory;

public record CreateAttachmentCategoryRequest
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int DisplayOrder { get; init; }
}
