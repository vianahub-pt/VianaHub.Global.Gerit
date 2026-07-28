namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDomain;

/// <summary>
/// Resposta resumida de um Domínio de Status (usada em listagens).
/// </summary>
public class StatusDomainResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
}
