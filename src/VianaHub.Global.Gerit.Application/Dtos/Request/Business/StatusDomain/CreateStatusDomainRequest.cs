namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.StatusDomain;

/// <summary>
/// DTO para criação de um Domínio de Status.
/// Name e Description são persistidos como tradução no idioma padrão (pt-PT).
/// </summary>
public class CreateStatusDomainRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
