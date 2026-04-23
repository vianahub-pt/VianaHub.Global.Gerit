namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContact;

/// <summary>
/// Request para atualização de VisitContact
/// </summary>
public class UpdateVisitContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
