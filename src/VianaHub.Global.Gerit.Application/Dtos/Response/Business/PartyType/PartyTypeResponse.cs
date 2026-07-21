namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.PartyType;

/// <summary>
/// Resposta resumida de um Tipo de Party (usada em listagens).
/// </summary>
public class PartyTypeResponse
{
    public byte Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LanguageCode { get; set; }
    public bool IsActive { get; set; }
}
