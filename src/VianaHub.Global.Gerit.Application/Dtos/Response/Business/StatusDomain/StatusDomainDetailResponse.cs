namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDomain;

/// <summary>
/// Resposta detalhada de um Domínio de Status (inclui traduções).
/// </summary>
public class StatusDomainDetailResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LanguageCode { get; set; }
    public bool IsActive { get; set; }
    public List<StatusDomainTranslationResponse>? Translations { get; set; }
}
