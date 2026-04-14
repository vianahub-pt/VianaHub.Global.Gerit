using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientContactEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ClientId { get; private set; }

        public string Name { get; private set; } = null!;
        public string PhoneNumber { get; private set; }
        public string CellPhoneNumber { get; private set; }
        public bool IsWhatsapp { get; private set; }
        public string Email { get; private set; } = null!;

        public bool IsPrimary { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public ClientEntity Client { get; private set; } = null!;

        // Construtor protegido para EF Core
        protected ClientContactEntity() { }

        public ClientContactEntity(int tenantId, int clientId, string name, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, bool isPrimary, int createdBy)
        {
            TenantId = tenantId;
            ClientId = clientId;
            Name = name;
            PhoneNumber = phoneNumber;
            CellPhoneNumber = cellPhoneNumber;
            IsWhatsapp = isWhatsapp;
            Email = email;
            IsPrimary = isPrimary;
            IsActive = true;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string phoneNumber, string cellPhoneNumber, bool isWhatsapp, string email, bool isPrimary, int modifiedBy)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            CellPhoneNumber = cellPhoneNumber;
            IsWhatsapp = isWhatsapp;
            Email = email;
            IsPrimary = isPrimary;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void SetPrimary(int modifiedBy)
        {
            IsPrimary = true;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void RemovePrimary(int modifiedBy)
        {
            IsPrimary = false;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Activate(int modifiedBy)
        {
            IsActive = true;
            IsActive = false;
            ModifiedBy = modifiedBy;
        }

        public void Deactivate(int modifiedBy)
        {
            IsActive = false;
            ModifiedBy = modifiedBy;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Delete(int modifiedBy)
        {
            if (IsDeleted)
                return;

            IsPrimary = false;
            IsActive = false;
            IsDeleted = true;
            ModifiedBy = modifiedBy;
                ModifiedAt = DateTime.UtcNow;
        }
    }
}