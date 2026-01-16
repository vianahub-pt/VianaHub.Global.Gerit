using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa um Tenant (inquilino) no sistema multi-tenant.
/// Aggregate Root para o contexto de Tenant.
/// </summary>
public class TenantEntity : Entity, IAggregateRoot
{
    public string LegalName { get; private set; }
    public string TradeName { get; private set; }
    public bool Consent { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    private readonly List<TenantContact> _contacts = new();
    public IReadOnlyCollection<TenantContact> Contacts => _contacts.AsReadOnly();

    private readonly List<TenantAddress> _addresses = new();
    public IReadOnlyCollection<TenantAddress> Addresses => _addresses.AsReadOnly();

    private readonly List<TenantFiscalDataEntity> _fiscalData = new();
    public IReadOnlyCollection<TenantFiscalDataEntity> FiscalData => _fiscalData.AsReadOnly();

    private readonly List<UserEntity> _users = new();
    public IReadOnlyCollection<UserEntity> Users => _users.AsReadOnly();

    // Construtor protegido para o EF Core
    protected TenantEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Tenant
    /// </summary>
    public TenantEntity(string legalName, string tradeName, bool consent = true)
    {
        SetLegalName(legalName);
        TradeName = tradeName;
        Consent = consent;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetLegalName(string legalName)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            throw new ArgumentException("Razão social não pode ser vazia.", nameof(legalName));

        if (legalName.Length > 200)
            throw new ArgumentException("Razão social não pode ter mais de 200 caracteres.", nameof(legalName));

        LegalName = legalName;
    }

    public void SetTradeName(string tradeName)
    {
        if (tradeName.Length > 200)
            throw new ArgumentException("Nome comercial não pode ter mais de 200 caracteres.", nameof(tradeName));

        TradeName = tradeName;
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

    public void AddContact(TenantContact contact)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact));

        _contacts.Add(contact);
    }

    public void AddAddress(TenantAddress address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _addresses.Add(address);
    }

    public void AddFiscalData(TenantFiscalDataEntity fiscalData)
    {
        if (fiscalData == null)
            throw new ArgumentNullException(nameof(fiscalData));

        _fiscalData.Add(fiscalData);
    }
}
