using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class InterventionTeamVehicleEntity : Entity
{
    public int TenantId { get; private set; }
    public int InterventionId { get; private set; }
    public int VehicleId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation
    public TenantEntity Tenant { get; private set; }
    public InterventionEntity Intervention { get; private set; }
    public VehicleEntity Vehicle { get; private set; }

    protected InterventionTeamVehicleEntity() { }

    public InterventionTeamVehicleEntity(int tenantId, int interventionId, int vehicleId, int createdBy)
    {
        TenantId = tenantId;
        InterventionId = interventionId;
        VehicleId = vehicleId;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int interventionId, int vehicleId, int modifiedBy)
    {
        InterventionId = interventionId;
        VehicleId = vehicleId;
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
