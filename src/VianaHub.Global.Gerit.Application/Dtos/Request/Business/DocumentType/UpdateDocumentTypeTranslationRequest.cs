namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.DocumentType;

/// <summary>
/// DTO para atualização de uma tradução de DocumentType.
/// </summary>
public class UpdateDocumentTypeTranslationRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
