using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientHierarchyEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ParentId { get; private set; }
        public int ChildId { get; private set; }
        public RelationshipType RelationshipType { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public ClientEntity ParentClient { get; private set; } = null!;
        public ClientEntity ChildClient { get; private set; } = null!;

        // Construtor protegido para EF Core
        protected ClientHierarchyEntity() { }

        public ClientHierarchyEntity(int tenantId, int parentId, int childId, RelationshipType relationshipType, int createdBy)
        {
            TenantId = tenantId;
            ParentId = parentId;
            ChildId = childId;
            RelationshipType = relationshipType;

            IsActive = true;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateRelationshipType(RelationshipType relationshipType, int modifiedBy)
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
            IsActive = false;
            IsDeleted = true;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }
    }
}