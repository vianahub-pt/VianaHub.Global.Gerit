namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.StatusDomain;

/// <summary>
/// Resposta detalhada de um Domínio de Status (inclui traduções).
/// </summary>
public class StatusDomainDetailResponse : StatusDomainResponse
{
    public List<StatusDomainTranslationResponse>? Translations { get; set; }
}
