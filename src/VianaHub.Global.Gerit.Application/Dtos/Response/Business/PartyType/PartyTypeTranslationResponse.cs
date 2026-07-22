namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.PartyType;

/// <summary>
/// Resposta de uma tradução de PartyType.
/// </summary>
public class PartyTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
