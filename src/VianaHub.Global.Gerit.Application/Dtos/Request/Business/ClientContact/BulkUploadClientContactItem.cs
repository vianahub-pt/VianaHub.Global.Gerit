namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;

public class BulkUploadClientContactItem
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
