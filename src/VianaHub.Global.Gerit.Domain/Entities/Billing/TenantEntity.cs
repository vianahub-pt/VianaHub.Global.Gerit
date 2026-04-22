using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa um Tenant (inquilino) no sistema multi-tenant.
/// Aggregate Root para o contexto de Tenant.
/// </summary>
public class TenantEntity : Entity, IAggregateRoot
{
    private readonly List<TenantContactEntity> _contacts = [];
    private readonly List<TenantAddressEntity> _addresses = [];
    private readonly List<TenantFiscalDataEntity> _fiscalData = [];
    private readonly List<UserEntity> _users = [];

    public TenantType TenantType { get; private set; }
    public OriginType OriginType { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Website { get; private set; }
    public string UrlImage { get; private set; }
    public string Note { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Partes internas do agregado
    public IReadOnlyCollection<TenantContactEntity> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<TenantAddressEntity> Addresses => _addresses.AsReadOnly();
    public IReadOnlyCollection<TenantFiscalDataEntity> FiscalData => _fiscalData.AsReadOnly();
    public IReadOnlyCollection<UserEntity> Users => _users.AsReadOnly();

    // Construtor protegido para o EF Core
    protected TenantEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Tenant
    /// </summary>
    public TenantEntity(TenantType tenantType, OriginType originType, string name, string email, string website, string urlImage, string note, int createdBy)
    {
        TenantType = tenantType;
        OriginType = originType;
        Name = name;
        Email = email;
        Website = website;
        UrlImage = urlImage;
        Note = note;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(TenantType tenantType, OriginType originType, string name, string email, string website, string urlImage, string note, int modifiedBy)
    {
        TenantType = tenantType;
        OriginType = originType;
        Name = name;
        Email = email;
        Website = website;
        UrlImage = urlImage;
        Note = note;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
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
