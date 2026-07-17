namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.DocumentType;

/// <summary>
/// DTO para criação de uma tradução de DocumentType.
/// </summary>
public class CreateDocumentTypeTranslationRequest
{
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
