namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberAddress;

/// <summary>
/// DTO para upload em lote de endereços
/// </summary>
public class BulkUploadTeamMemberAddressItem
{
    public int TeamMemberId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string District { get; set; }
    public string CountryCode { get; set; }
    public bool IsPrimary { get; set; }
}
