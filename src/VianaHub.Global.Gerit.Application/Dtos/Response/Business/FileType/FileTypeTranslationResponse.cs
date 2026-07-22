namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.FileType;

/// <summary>
/// Resposta de uma tradução de FileType por idioma.
/// </summary>
public class FileTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
