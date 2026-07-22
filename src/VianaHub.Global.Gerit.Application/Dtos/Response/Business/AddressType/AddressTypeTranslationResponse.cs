namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.AddressType;

/// <summary>
/// Resposta de uma tradução de AddressType por idioma.
/// </summary>
public class AddressTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
