namespace VianaHub.Global.Gerit.Application.Dtos.Response.Business.Visit;

public class VisitResponse
{
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? Title { get; set; }
    public DateTime StartDateTime { get; set; }
    public bool IsActive { get; set; }
}
