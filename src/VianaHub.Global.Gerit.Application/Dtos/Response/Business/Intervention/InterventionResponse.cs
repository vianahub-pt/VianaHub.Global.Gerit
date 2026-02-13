namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Intervention;

public class InterventionResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int ClientId { get; set; }
    public string Client { get; set; }
    public int TeamMemberId { get; set; }
    public string TeamMember { get; set; }
    public int VehicleId { get; set; }
    public string Plate { get; set; }
    public int InterventionStatusId { get; set; }
    public string InterventionStatus { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public decimal EstimatedValue { get; set; }
    public decimal? RealValue { get; set; }
    public bool IsActive { get; set; }
}
