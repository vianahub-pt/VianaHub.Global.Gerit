namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.PartyType;

/// <summary>
/// Resposta detalhada de um Tipo de Party (inclui traduções).
/// Classe independente — não herda de PartyTypeResponse.
/// </summary>
public class PartyTypeDetailResponse
{
    public byte Id { get; set; }
    public string? Code { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
