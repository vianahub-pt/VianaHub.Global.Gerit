using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa consentimentos LGPD/GDPR de clientes
/// </summary>
public class ClientConsentsEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public int ConsentTypeId { get; private set; }
    public bool Granted { get; private set; }
    public DateTime GrantedDate { get; private set; }
    public DateTime? RevokedDate { get; private set; }
    public string Origin { get; private set; } // Web, Mobile, Paper, API
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity Client { get; private set; }
    public ConsentTypeEntity ConsentType { get; private set; }

    protected ClientConsentsEntity() { }

    public ClientConsentsEntity(int tenantId, int clientId, int consentTypeId, bool granted,
        DateTime grantedDate, string origin, string ipAddress, string userAgent, int createdBy)
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

    public void RevokeConsent(int modifiedBy)
    {
        Granted = false;
        RevokedDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void GrantConsent(int modifiedBy)
    {
        Granted = true;
        RevokedDate = null;
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
