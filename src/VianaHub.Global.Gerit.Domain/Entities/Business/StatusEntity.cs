using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Status de Intervenção
/// Aggregate Root para o contexto de Status de Intervenção
/// </summary>
public class StatusEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public int StatusTypeId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected StatusEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Status de Intervenção
    /// </summary>
    public StatusEntity(int tenantId, int statusTypeId, string name, string description, int createdBy)
    {
        TenantId = tenantId;
        StatusTypeId = statusTypeId;
        Name = name;
        Description = description;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int statusTypeId, string name, string description, int modifiedBy)
    {
        StatusTypeId = statusTypeId;
        Name = name;
        Description = description;
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
