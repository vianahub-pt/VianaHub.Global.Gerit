namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.PartyType;

/// <summary>
/// Resposta de tradução de um Tipo de Party por idioma.
/// </summary>
public class PartyTypeTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
