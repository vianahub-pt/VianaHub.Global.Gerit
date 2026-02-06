using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Cliente do Tenant
/// Aggregate Root para o contexto de Cliente
/// </summary>
public class ClientEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public bool Consent { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    private readonly List<ClientContactEntity> _contacts = new();
    public IReadOnlyCollection<ClientContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<ClientAddressEntity> _addresses = new();
    public IReadOnlyCollection<ClientAddressEntity> Addresses => _addresses.AsReadOnly();

    private readonly List<ClientFiscalDataEntity> _fiscalData = new();
    public IReadOnlyCollection<ClientFiscalDataEntity> FiscalData => _fiscalData.AsReadOnly();

    // Construtor protegido para o EF Core
    protected ClientEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Cliente
    /// </summary>
    public ClientEntity(int tenantId, string name, string phone, string email, bool consent, int createdBy)
    {
        TenantId = tenantId;
        Name = name;
        Phone = phone;
        Email = email;
        Consent = consent;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string phone, string email, bool consent, int modifiedBy)
    {
        Name = name;
        Phone = phone;
        Email = email;
        Consent = consent;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateConsent(bool consent, int modifiedBy)
    {
        Consent = consent;
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

    public void AddFiscalData(ClientFiscalDataEntity fiscalData, int createdBy)
    {
        if (fiscalData == null)
            throw new ArgumentNullException(nameof(fiscalData));

        _fiscalData.Add(fiscalData);
    }
}
