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
    public ClientEntity(int tenantId, string name, string phone, string email = null, bool consent = true)
    {
        TenantId = tenantId;
        SetName(name);
        SetPhone(phone);
        Email = email;
        Consent = consent;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(name));

        if (name.Length > 150)
            throw new ArgumentException("Nome não pode ter mais de 150 caracteres.", nameof(name));

        Name = name;
    }

    public void SetEmail(string email)
    {
        if (email != null && email.Length > 255)
            throw new ArgumentException("Email não pode ter mais de 255 caracteres.", nameof(email));

        Email = email;
    }

    public void SetPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Telefone não pode ser vazio.", nameof(phone));

        if (phone.Length > 50)
            throw new ArgumentException("Telefone não pode ter mais de 50 caracteres.", nameof(phone));

        Phone = phone;
    }

    public void UpdateConsent(bool consent)
    {
        Consent = consent;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
    }

    public void AddContact(ClientContactEntity contact)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact));

        _contacts.Add(contact);
    }

    public void AddAddress(ClientAddressEntity address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _addresses.Add(address);
    }

    public void AddFiscalData(ClientFiscalDataEntity fiscalData)
    {
        if (fiscalData == null)
            throw new ArgumentNullException(nameof(fiscalData));

        _fiscalData.Add(fiscalData);
    }
}
