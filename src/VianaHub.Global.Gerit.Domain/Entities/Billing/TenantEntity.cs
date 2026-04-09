using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Identity;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa um Tenant (inquilino) no sistema multi-tenant.
/// Aggregate Root para o contexto de Tenant.
/// </summary>
public class TenantEntity : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public bool Consent { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    private readonly List<TenantContactEntity> _contacts = new();
    public IReadOnlyCollection<TenantContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<TenantAddressEntity> _addresses = new();
    public IReadOnlyCollection<TenantAddressEntity> Addresses => _addresses.AsReadOnly();

    private readonly List<TenantFiscalDataEntity> _fiscalData = new();
    public IReadOnlyCollection<TenantFiscalDataEntity> FiscalData => _fiscalData.AsReadOnly();

    private readonly List<UserEntity> _users = new();
    public IReadOnlyCollection<UserEntity> Users => _users.AsReadOnly();

    // Construtor protegido para o EF Core
    protected TenantEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Tenant
    /// </summary>
    public TenantEntity(string name, bool consent, int createdBy)
    {
        Name = name;
        Consent = consent;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, bool consent, int modifiedBy)
    {
        Name = name;
        Consent = consent;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
    public void UpdateConsent(bool consent)
    {
        Consent = consent;
    }

    public void Activate(int? modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int? modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int? modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void AddAddress(TenantAddressEntity address, int createdBy)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _addresses.Add(address);
    }

    public void AddFiscalData(TenantFiscalDataEntity fiscalData, int createdBy)
    {
        if (fiscalData == null)
            throw new ArgumentNullException(nameof(fiscalData));

        _fiscalData.Add(fiscalData);
    }
}
