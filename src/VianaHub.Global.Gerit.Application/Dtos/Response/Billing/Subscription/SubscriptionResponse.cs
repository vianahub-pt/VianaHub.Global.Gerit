namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Subscription;

public class SubscriptionResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int PlanId { get; set; }
    public string PlanName { get; set; }
    public string StripeId { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public DateTime? TrialStart { get; set; }
    public DateTime? TrialEnd { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
    public DateTime? CanceledAt { get; set; }
    public string CancellationReason { get; set; }
    public string StripeCustomerId { get; set; }
    public bool IsActive { get; set; }
    public bool IsTrial { get; set; }
    public int DaysRemaining { get; set; }
}
