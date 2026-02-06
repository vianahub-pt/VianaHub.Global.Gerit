using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Equipamento
/// </summary>
public class EquipmentEntity : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string SerialNumber { get; private set; }
    public TypeEquipament TypeEquipament { get; private set; }
    public EquipmentStatus Status { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected EquipmentEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Equipamento
    /// </summary>
    public EquipmentEntity(int tenantId, string name, TypeEquipament typeEquipament, string serialNumber, int createdBy)
    {
        TenantId = tenantId;
        Name = name;
        TypeEquipament = typeEquipament;
        Status = EquipmentStatus.Available;
        SerialNumber = serialNumber;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, TypeEquipament typeEquipament, string serialNumber, int modifiedBy)
    {
        Name = name;
        TypeEquipament = typeEquipament;
        SerialNumber = serialNumber;
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
