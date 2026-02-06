using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Helpers;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa um contato do Tenant
/// </summary>
public class TenantContact : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected TenantContact() { }

    /// <summary>
    /// Construtor para criação de um novo contato do Tenant
    /// </summary>
    public TenantContact(int tenantId, string name, string email, string phone, bool isPrimary, int createdBy)
    {
        TenantId = tenantId;
        Name = name;
        Email = email;
        Phone = phone;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone, int modifiedBy)
    {
        Name = name;
        Email = email;
        Phone = phone;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void RemoveAsPrimary()
    {
        IsPrimary = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
    }
}
