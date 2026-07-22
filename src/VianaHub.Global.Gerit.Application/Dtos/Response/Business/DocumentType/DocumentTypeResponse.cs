namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.DocumentType;

/// <summary>
/// Resposta resumida de um Tipo de Documento (usada em listagens).
/// </summary>
public class DocumentTypeResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
