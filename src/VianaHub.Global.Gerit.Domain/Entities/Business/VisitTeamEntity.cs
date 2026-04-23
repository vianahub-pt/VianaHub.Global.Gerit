using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class VisitTeamEntity : Entity
{
    public int TenantId { get; private set; }
    public int VisitId { get; private set; }
    public int TeamId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation
    public TenantEntity Tenant { get; private set; }
    public VisitEntity Visit { get; private set; }
    public TeamEntity Team { get; private set; }

    protected VisitTeamEntity() { }

    public VisitTeamEntity(int tenantId, int interventionId, int teamId, int createdBy)
    {
        TenantId = tenantId;
        VisitId = interventionId;
        TeamId = teamId;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int interventionId, int teamId, int modifiedBy)
    {
        VisitId = interventionId;
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
