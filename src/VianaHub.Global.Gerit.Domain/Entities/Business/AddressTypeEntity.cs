using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Tipo de Endereço (Residencial, Comercial, etc.)
/// </summary>
public class AddressTypeEntity : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public Billing.TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected AddressTypeEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Tipo de Endereço
    /// </summary>
    public AddressTypeEntity(int tenantId, string name, int createdBy)
    {
        TenantId = tenantId;
        Name = name;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, int modifiedBy)
    {
        Name = name;
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
