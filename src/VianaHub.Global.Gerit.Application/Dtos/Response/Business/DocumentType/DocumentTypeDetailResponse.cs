namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;

/// <summary>
/// Resposta detalhada de um Tipo de Documento (inclui traduções).
/// </summary>
public class DocumentTypeDetailResponse : DocumentTypeResponse
{
    public List<DocumentTypeTranslationResponse>? Translations { get; set; }
}
