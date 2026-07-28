namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContactPersons;

/// <summary>
/// Modelo para upload em lote de VisitContact via CSV
/// </summary>
public class BulkUploadVisitContactPersonItem
{
    public int VisitId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsCellPhoneWhatsapp { get; set; }
    public bool IsPrimary { get; set; }
}
