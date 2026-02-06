namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberAddress;

/// <summary>
/// DTO para atualização de endereço de membro da equipe
/// </summary>
public class UpdateTeamMemberAddressRequest
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string District { get; set; }
    public string CountryCode { get; set; }
}
