namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.AcquisitionSourceType;

/// <summary>
/// Resposta de uma tradução de AcquisitionSourceType por idioma.
/// </summary>
public class AcquisitionSourceTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
