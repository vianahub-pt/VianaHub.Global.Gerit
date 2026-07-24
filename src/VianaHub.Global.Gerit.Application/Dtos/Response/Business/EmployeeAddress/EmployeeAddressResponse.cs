namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.EmployeeAddress;

/// <summary>
/// DTO de resposta para endereþo de membro da equipe
/// </summary>
public class EmployeeAddressResponse
{
    public int Id { get; set; }
    public string? AddressTypeName { get; set; }
    public string? CountryCode { get; set; }
    public string? Street { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
