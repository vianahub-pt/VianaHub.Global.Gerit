using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Business
{
    public class ClientConsentsEntity : Entity
    {
        public int TenantId { get; private set; }
        public int ClientId { get; private set; }
        public int ConsentTypeId { get; private set; }

        public bool Granted { get; private set; }
        public DateTime GrantedDate { get; private set; }
        public DateTime? RevokedDate { get; private set; }

        public string Origin { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public ClientEntity Client { get; private set; } = null!;
        public ConsentTypeEntity ConsentType { get; private set; } = null!;

        // Construtor protegido para EF Core
        protected ClientConsentsEntity() { }

        public ClientConsentsEntity(int tenantId, int clientId, int consentTypeId, bool granted, DateTime grantedDate, string origin, string ipAddress, string userAgent, int createdBy)
        {
            TenantId = tenantId;
            ClientId = clientId;
            ConsentTypeId = consentTypeId;
            Granted = granted;
            GrantedDate = grantedDate;
            Origin = origin;
            IpAddress = ipAddress;
            UserAgent = userAgent;

            IsActive = true;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsRevoked => RevokedDate.HasValue;
        public bool IsGrantedAndActive => Granted && !IsDeleted && IsActive && !IsRevoked;

        public void Update(string origin, string ipAddress, string userAgent, int modifiedBy)
        {
            Origin = origin;
            IpAddress = ipAddress;
            UserAgent = userAgent;
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

        public void Revoke(DateTime revokedDate, int modifiedBy)
        {
            RevokedDate = revokedDate;
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