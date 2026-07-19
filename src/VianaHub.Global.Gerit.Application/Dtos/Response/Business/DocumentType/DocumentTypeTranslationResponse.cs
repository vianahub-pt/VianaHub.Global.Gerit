namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;

/// <summary>
/// Resposta de uma tradução de DocumentType.
/// </summary>
public class DocumentTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
