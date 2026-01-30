namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Subscription;

public class CreateSubscriptionRequest
{
    public int TenantId { get; set; }
    public int PlanId { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public DateTime? TrialStart { get; set; }
    public DateTime? TrialEnd { get; set; }
    public string StripeCustomerId { get; set; }
}
