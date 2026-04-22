namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;

/// <summary>
/// Item de upload em massa para Client
/// </summary>
public class BulkUploadClientItem
{
    public int ClientType { get; set; }
    public int OriginType { get; set; }
    public int Origin { get; set; }
    public string UrlImage { get; set; }
    public string Note { get; set; }
}
