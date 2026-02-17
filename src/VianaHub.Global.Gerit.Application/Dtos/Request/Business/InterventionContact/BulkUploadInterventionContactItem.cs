namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;

/// <summary>
/// Modelo para upload em lote de InterventionContact via CSV
/// </summary>
public class BulkUploadInterventionContactItem
{
    public int InterventionId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
