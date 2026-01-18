using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa uma assinatura de um tenant a um plano
/// </summary>
public class SubscriptionEntity : Entity
{
    public int TenantId { get; private set; }
    public int PlanId { get; private set; }
    public string StripeId { get; private set; }
    public DateTime CurrentPeriodStart { get; private set; }
    public DateTime CurrentPeriodEnd { get; private set; }
    public DateTime? TrialStart { get; private set; }
    public DateTime? TrialEnd { get; private set; }
    public bool CancelAtPeriodEnd { get; private set; }
    public DateTime? CanceledAt { get; private set; }
    public string CancellationReason { get; private set; }
    public string StripeCustomerId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public PlanEntity Plan { get; private set; }

    // Construtor protegido para o EF Core
    protected SubscriptionEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova assinatura
    /// </summary>
    public SubscriptionEntity(
        int tenantId,
        int planId,
        DateTime currentPeriodStart,
        DateTime currentPeriodEnd,
        int createdBy,
        DateTime? trialStart = null,
        DateTime? trialEnd = null,
        string stripeCustomerId = null)
    {
        TenantId = tenantId;
        PlanId = planId;
        CurrentPeriodStart = currentPeriodStart;
        CurrentPeriodEnd = currentPeriodEnd;
        TrialStart = trialStart;
        TrialEnd = trialEnd;
        StripeCustomerId = stripeCustomerId;
        CancelAtPeriodEnd = false;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
    }

    public void Update(
        int planId,
        DateTime currentPeriodStart,
        DateTime currentPeriodEnd,
        int modifiedBy,
        DateTime? trialStart = null,
        DateTime? trialEnd = null)
    {
        PlanId = planId;
        CurrentPeriodStart = currentPeriodStart;
        CurrentPeriodEnd = currentPeriodEnd;
        TrialStart = trialStart;
        TrialEnd = trialEnd;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateStripeData(string stripeId, string stripeCustomerId, int modifiedBy)
    {
        StripeId = stripeId;
        StripeCustomerId = stripeCustomerId;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(int? modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int? modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int? modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Cancel(string cancellationReason, bool cancelAtPeriodEnd, int modifiedBy)
    {
        CancelAtPeriodEnd = cancelAtPeriodEnd;
        CanceledAt = DateTime.UtcNow;
        CancellationReason = cancellationReason;
        
        if (!cancelAtPeriodEnd)
        {
            IsActive = false;
        }
        
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Renew(DateTime newPeriodEnd, int modifiedBy)
    {
        CurrentPeriodStart = CurrentPeriodEnd;
        CurrentPeriodEnd = newPeriodEnd;
        CancelAtPeriodEnd = false;
        CanceledAt = null;
        CancellationReason = null;
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
