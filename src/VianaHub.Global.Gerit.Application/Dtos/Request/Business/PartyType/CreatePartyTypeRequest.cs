namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.PartyType;

/// <summary>
/// DTO para criação de um Tipo de Party (Pessoa Física / Jurídica).
/// Name e Description são persistidos como tradução no idioma padrão (pt-PT).
/// </summary>
public class CreatePartyTypeRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
