namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeContact;

/// <summary>
/// Item para bulk upload de EmployeeContacts via CSV
/// </summary>
public class BulkUploadEmployeeContactItem
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsPrimary { get; set; }
}
