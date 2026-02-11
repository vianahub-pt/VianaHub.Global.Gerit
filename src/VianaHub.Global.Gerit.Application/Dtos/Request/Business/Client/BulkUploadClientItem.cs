namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Item de upload em massa para Client
/// </summary>
public class BulkUploadClientItem
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool Consent { get; set; }
}
