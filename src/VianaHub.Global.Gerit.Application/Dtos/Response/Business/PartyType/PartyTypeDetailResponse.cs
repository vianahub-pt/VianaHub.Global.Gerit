namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.PartyType;

/// <summary>
/// Resposta detalhada de um Tipo de Party (inclui traduções).
/// </summary>
public class PartyTypeDetailResponse : PartyTypeResponse
{
    public List<PartyTypeTranslationResponse>? Translations { get; set; }
}
