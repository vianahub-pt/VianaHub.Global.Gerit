namespace VianaHub.Global.Gerit.Application.Dtos.Request.Subscription;

public class UpdateSubscriptionRequest
{
    public int PlanId { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public DateTime? TrialStart { get; set; }
    public DateTime? TrialEnd { get; set; }
}
