namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;

public class ClientConsentsResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientId { get; set; }
    public int ConsentTypeId { get; set; }
    public bool Granted { get; set; }
    public DateTime GrantedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string Origin { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
