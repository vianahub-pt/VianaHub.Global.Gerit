using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientHierarchyEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ParentClientId { get; private set; }
        public int ChildClientId { get; private set; }
        public int RelationshipType { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public ClientEntity ParentClient { get; private set; } = null!;
        public ClientEntity ChildClient { get; private set; } = null!;

        // Construtor protegido para EF Core
        protected ClientHierarchyEntity() { }

        public ClientHierarchyEntity(int tenantId, int parentClientId, int childClientId, int relationshipType, int createdBy)
        {
            TenantId = tenantId;
            ParentClientId = parentClientId;
            ChildClientId = childClientId;
            RelationshipType = relationshipType;

            IsActive = true;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateRelationshipType(int relationshipType, int modifiedBy)
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