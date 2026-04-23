using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um contato da Intervenção
/// </summary>
public class VisitContactEntity : Entity
{
    public int TenantId { get; private set; }
    public int VisitId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public VisitEntity Visit { get; private set; }

    // Construtor protegido para o EF Core
    protected VisitContactEntity() { }

    /// <summary>
    /// Construtor para criação de um novo contato da Intervenção
    /// </summary>
    public VisitContactEntity(int tenantId, int interventionId, string name, string email, string phone,
        bool isPrimary, int createdBy)
    {
        TenantId = tenantId;
        VisitId = interventionId;
        Name = name;
        Email = email;
        Phone = phone;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone, bool isPrimary, int modifiedBy)
    {
        Name = name;
        Email = email;
        Phone = phone;
        IsPrimary = isPrimary;
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
