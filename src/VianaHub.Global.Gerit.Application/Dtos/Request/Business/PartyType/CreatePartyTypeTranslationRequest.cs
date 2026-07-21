namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.PartyType;

/// <summary>
/// DTO para criação de uma tradução de PartyType.
/// </summary>
public class CreatePartyTypeTranslationRequest
{
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
