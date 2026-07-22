namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;

/// <summary>
/// Resposta detalhada de um Tipo de Documento (inclui traduções).
/// </summary>
public class DocumentTypeDetailResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<DocumentTypeTranslationResponse>? Translations { get; set; }
}
