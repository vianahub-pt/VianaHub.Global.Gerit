namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.VisitTeamEquipments;

public class VisitTeamEquipmentDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int VisitTeamId { get; set; }
    public string? VisitTeam { get; set; }
    public int EquipmentId { get; set; }
    public string? Equipment { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
