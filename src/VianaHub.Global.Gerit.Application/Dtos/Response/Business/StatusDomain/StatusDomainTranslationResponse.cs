namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDomain;

/// <summary>
/// Resposta de tradução de um Domínio de Status por idioma.
/// </summary>
public class StatusDomainTranslationResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
