namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Plan;

public class PlanDetailResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public decimal? PricePerMonth { get; set; }
    public decimal? PricePerYear { get; set; }
    public string? Currency { get; set; }
    public int MaxUsers { get; set; }
    public int MaxPhotosPerVisit { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
