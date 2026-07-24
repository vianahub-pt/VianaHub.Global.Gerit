namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;

/// <summary>
/// Resposta de tradução de um Tipo de Documento por idioma.
/// </summary>
public class DocumentTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
