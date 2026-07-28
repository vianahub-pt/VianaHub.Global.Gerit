namespace VianaHub.Global.Gerit.Application.Dtos.Response.Billing.Subscription;

public class SubscriptionResponse
{
    public int Id { get; set; }
    public string? SubscriptionPlanName { get; set; }
    public string? StatusDomainName { get; set; }
    public bool IsActive { get; set; }
}
