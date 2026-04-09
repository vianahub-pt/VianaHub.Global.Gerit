namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContact;

/// <summary>
/// Modelo para upload em lote de VisitContact via CSV
/// </summary>
public class BulkUploadVisitContactItem
{
    public int VisitId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
