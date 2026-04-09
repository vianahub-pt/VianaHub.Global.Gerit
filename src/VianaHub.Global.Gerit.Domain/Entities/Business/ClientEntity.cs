using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Cliente do Tenant
/// Aggregate Root para o contexto de Cliente
/// </summary>
public class ClientEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public ClientType ClientType { get; private set; }
    public Origin Origin { get; private set; }
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string Website { get; private set; }
    public string UrlImage { get; private set; }
    public int? Score { get; private set; }
    public bool Consent { get; private set; }
    public DateTime ConsentDate { get; private set; }
    public DateTime? RevokedConsentDate { get; private set; }
    public string Remarks { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    private readonly List<ClientContactEntity> _contacts = new();
    public IReadOnlyCollection<ClientContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<ClientAddressEntity> _addresses = new();
    public IReadOnlyCollection<ClientAddressEntity> Addresses => _addresses.AsReadOnly();

    // Construtor protegido para o EF Core
    protected ClientEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Cliente
    /// </summary>
    public ClientEntity(int tenantId, ClientType clientType, Origin origin, string name, string phone, string email, string website, string urlImage, int? score, bool consent, DateTime consentDate, string remarks, int createdBy)
    {
        TenantId = tenantId;
        ClientType = clientType;
        Origin = origin;
        Name = name;
        Phone = phone;
        Email = email;
        Website = website;
        UrlImage = urlImage;
        Score = score;
        Consent = consent;
        ConsentDate = consentDate;
        Remarks = remarks;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(ClientType clientType, Origin origin, string name, string phone, string email, string website, string urlImage, int? score, bool consent, string remarks, int modifiedBy)
    {
        ClientType = clientType;
        Origin = origin;
        Name = name;
        Phone = phone;
        Email = email;
        Website = website;
        UrlImage = urlImage;
        Score = score;
        Consent = consent;
        Remarks = remarks;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateConsent(bool consent, int modifiedBy)
    {
        Consent = consent;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RevokeConsent(int modifiedBy)
    {
        Consent = false;
        RevokedConsentDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RestoreConsent(int modifiedBy)
    {
        Consent = true;
        RevokedConsentDate = null;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateScore(int score, int modifiedBy)
    {
        Score = score;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpadateImage(string urlImage, int modifiedBy)
    {
        UrlImage = urlImage;
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

    public void AddContact(ClientContactEntity contact, int createdBy)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact));

        _contacts.Add(contact);
    }

    public void AddAddress(ClientAddressEntity address, int createdBy)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _addresses.Add(address);
    }
}
