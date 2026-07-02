namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Status;

/// <summary>
/// Item CSV para bulk upload de Status de Intervenção
/// </summary>
public class BulkUploadStatusItem
{
    public int StatusTypeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
