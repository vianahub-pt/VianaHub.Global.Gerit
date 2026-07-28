namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContactPersons;

/// <summary>
/// Response de VisitContact
/// </summary>
public class VisitContactPersonResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
