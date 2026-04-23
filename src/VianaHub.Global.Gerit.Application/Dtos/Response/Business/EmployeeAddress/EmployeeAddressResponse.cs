namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeAddress;

/// <summary>
/// DTO de resposta para endereþo de membro da equipe
/// </summary>
public class EmployeeAddressResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int EmployeeId { get; set; }
    public string Employee { get; set; }
    public int AddressTypeId { get; set; }
    public string AddressType { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string District { get; set; }
    public string CountryCode { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
