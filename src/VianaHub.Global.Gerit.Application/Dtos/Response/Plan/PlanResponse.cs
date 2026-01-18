namespace VianaHub.Global.Gerit.Application.Dtos.Response.Plan;

public class PlanResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public decimal? PricePerMonth { get; set; }
    public decimal? PricePerYear { get; set; }
    public string Currency { get; set; }
    public int MaxUsers { get; set; }
    public int MaxPhotosPerInterventions { get; set; }
    public bool IsActive { get; set; }
}
