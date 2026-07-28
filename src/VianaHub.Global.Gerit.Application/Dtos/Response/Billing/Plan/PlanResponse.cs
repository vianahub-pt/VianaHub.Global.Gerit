namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Plan;

public class PlanResponse
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public decimal? PricePerMonth { get; set; }
    public bool IsActive { get; set; }
}
