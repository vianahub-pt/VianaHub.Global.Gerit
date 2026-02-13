namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;

public class BulkUploadClientContactItem
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
