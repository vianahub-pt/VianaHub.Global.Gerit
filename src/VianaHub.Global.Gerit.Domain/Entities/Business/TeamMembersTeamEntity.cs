using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class TeamMembersTeamEntity : Entity
{
    public int TenantId { get; private set; }
    public int TeamId { get; private set; }
    public int TeamMemberId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation
    public TenantEntity Tenant { get; private set; }
    public TeamEntity Team { get; private set; }
    public TeamMemberEntity TeamMember { get; private set; }

    protected TeamMembersTeamEntity() { }

    public TeamMembersTeamEntity(int tenantId, int teamId, int teamMemberId, int createdBy)
    {
        TenantId = tenantId;
        TeamId = teamId;
        TeamMemberId = teamMemberId;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int teamId, int teamMemberId, int modifiedBy)
    {
        TeamId = teamId;
        TeamMemberId = teamMemberId;
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
