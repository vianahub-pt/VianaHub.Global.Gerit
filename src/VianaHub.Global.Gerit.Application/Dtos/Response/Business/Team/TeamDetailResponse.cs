namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Team;

public class TeamDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
