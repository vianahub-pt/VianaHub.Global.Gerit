using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class InterventionTeamEntity : Entity
{
    public int TenantId { get; private set; }
    public int InterventionId { get; private set; }
    public int TeamId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation
    public TenantEntity Tenant { get; private set; }
    public InterventionEntity Intervention { get; private set; }
    public TeamEntity Team { get; private set; }

    protected InterventionTeamEntity() { }

    public InterventionTeamEntity(int tenantId, int interventionId, int teamId, int createdBy)
    {
        TenantId = tenantId;
        InterventionId = interventionId;
        TeamId = teamId;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int interventionId, int teamId, int modifiedBy)
    {
        InterventionId = interventionId;
        TeamId = teamId;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(int modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
