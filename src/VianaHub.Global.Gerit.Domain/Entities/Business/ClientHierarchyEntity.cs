using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa hierarquia entre clientes (filiais, subsidiárias)
/// </summary>
public class ClientHierarchyEntity : Entity
{
    public int TenantId { get; private set; }
    public int ParentClientId { get; private set; }
    public int ChildClientId { get; private set; }
    public int RelationshipType { get; private set; } // 1=Branch, 2=Subsidiary
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity ParentClient { get; private set; }
    public ClientEntity ChildClient { get; private set; }

    protected ClientHierarchyEntity() { }

    public ClientHierarchyEntity(int tenantId, int parentClientId, int childClientId, int relationshipType, int createdBy)
    {
        if (parentClientId == childClientId)
            throw new InvalidOperationException("client_hierarchy.same_client");

        TenantId = tenantId;
        ParentClientId = parentClientId;
        ChildClientId = childClientId;
        RelationshipType = relationshipType;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int relationshipType, int modifiedBy)
    {
        RelationshipType = relationshipType;
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
