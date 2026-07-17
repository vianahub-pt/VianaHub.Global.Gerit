namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.DocumentType;

/// <summary>
/// DTO para criação de um Tipo de Documento (BI, Passaporte, NIF, etc.).
/// </summary>
public class CreateDocumentTypeRequest
{
    public string? Code { get; set; }
    public List<CreateDocumentTypeTranslationRequest>? Translations { get; set; }
}
