namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.AttachmentCategory;

public record AttachmentCategoryResponse
{
    public int Id { get; init; }
    public int TenantId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsSystem { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
}
