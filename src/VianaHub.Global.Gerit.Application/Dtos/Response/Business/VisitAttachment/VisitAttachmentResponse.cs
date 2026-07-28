namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitAttachment;

public record VisitAttachmentResponse
{
    public int Id { get; init; }
    public string? FileTypeName { get; init; }
    public string? MimeType { get; init; }
    public string? FileName { get; init; }
    public bool IsPrimary { get; init; }
    public bool IsActive { get; init; }
}
