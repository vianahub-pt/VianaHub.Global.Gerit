namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientContact;

/// <summary>
/// Response para ClientContact
/// </summary>
public class ClientContactResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}
