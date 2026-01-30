namespace VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Subscription;

public class CancelSubscriptionRequest
{
    public string CancellationReason { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
}
