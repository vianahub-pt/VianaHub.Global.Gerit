using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa uma categoria de anexo para organização de ficheiros
/// Permite categorizar anexos (fotos, documentos, relatórios, etc.)
/// </summary>
public class AttachmentCategoryEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsSystem { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    protected AttachmentCategoryEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova categoria de anexo
    /// </summary>
    public AttachmentCategoryEntity(int tenantId, string name, string description, int displayOrder, bool isSystem, int createdBy)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        IsSystem = isSystem;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, int displayOrder, int modifiedBy)
    {
        if (IsSystem)
            throw new InvalidOperationException("attachment_category.system_category_cannot_be_modified");

        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateDisplayOrder(int displayOrder, int modifiedBy)
    {
        DisplayOrder = displayOrder;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate(int modifiedBy)
    {
        if (IsSystem)
            throw new InvalidOperationException("attachment_category.system_category_cannot_be_deactivated");

        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int modifiedBy)
    {
        if (IsSystem)
            throw new InvalidOperationException("attachment_category.system_category_cannot_be_deactivated");

        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int modifiedBy)
    {
        if (IsSystem)
            throw new InvalidOperationException("attachment_category.system_category_cannot_be_deleted");

        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
