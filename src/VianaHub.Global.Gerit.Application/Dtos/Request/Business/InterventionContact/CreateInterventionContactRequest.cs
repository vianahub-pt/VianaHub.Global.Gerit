namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContact;

/// <summary>
/// Request para criação de VisitContact
/// </summary>
public class CreateVisitContactRequest
{
    public int VisitId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsPrimary { get; set; }
}
