namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusDomain;

/// <summary>
/// DTO para atualização de uma tradução de StatusDomain.
/// </summary>
public class UpdateStatusDomainTranslationRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
