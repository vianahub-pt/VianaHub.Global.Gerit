namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContact;

/// <summary>
/// Response de VisitContact
/// </summary>
public class VisitContactResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitId { get; set; }
    public string Visit { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
