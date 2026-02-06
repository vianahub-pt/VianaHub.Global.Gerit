using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um contato do Cliente
/// </summary>
public class ClientContactEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity Client { get; private set; }

    // Construtor protegido para o EF Core
    protected ClientContactEntity() { }

    /// <summary>
    /// Construtor para criação de um novo contato do Cliente
    /// </summary>
    public ClientContactEntity(int tenantId, int clientId, string name, string email, string phone, bool isPrimary, int createdBy)
    {
        TenantId = tenantId;
        ClientId = clientId;
        Name = name;
        Email = email;
        Phone = phone;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string name, string email, string phone, int modifiedBy)
    {
        Name = name;
        Email = email;
        Phone = phone;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetAsPrimary(int modifiedBy)
    {
        IsPrimary = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RemoveAsPrimary(int modifiedBy)
    {
        IsPrimary = false;
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
