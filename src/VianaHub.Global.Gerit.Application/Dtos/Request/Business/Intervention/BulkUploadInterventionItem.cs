namespace VianaHub.Global.Gerit.Application.Dtos.Request.Business.Visit;

public class BulkUploadVisitItem
{
    public int ClientId { get; set; }
    public int StatusId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public decimal EstimatedValue { get; set; }
}
