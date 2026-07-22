namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusDomain;

/// <summary>
/// DTO para criação de uma tradução de StatusDomain.
/// </summary>
public class CreateStatusDomainTranslationRequest
{
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
