namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Intervention;

public class BulkUploadInterventionItem
{
    public int ClientId { get; set; }
    public int TeamMemberId { get; set; }
    public int VehicleId { get; set; }
    public int InterventionStatusId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public decimal EstimatedValue { get; set; }
}
