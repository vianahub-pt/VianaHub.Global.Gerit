namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitContactPersons;

/// <summary>
/// DTO de resposta detalhada para VisitContact — inclui campos de auditoria.
/// Classe independente — não herda de VisitContactResponse.
/// </summary>
public class VisitContactPersonDetailResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CellPhoneNumber { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
