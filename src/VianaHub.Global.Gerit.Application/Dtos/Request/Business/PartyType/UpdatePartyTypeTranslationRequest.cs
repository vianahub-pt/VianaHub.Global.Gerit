namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.PartyType;

/// <summary>
/// DTO para atualização de uma tradução de PartyType.
/// </summary>
public class UpdatePartyTypeTranslationRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
