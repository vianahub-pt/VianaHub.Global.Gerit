using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa um endereço do Tenant
/// </summary>
public class TenantAddress : Entity
{
    public int TenantId { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string District { get; private set; }
    public string CountryCode { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected TenantAddress() { }

    /// <summary>
    /// Construtor para criação de um novo endereço do Tenant
    /// </summary>
    public TenantAddress(int tenantId, string street, string city, string postalCode, string district, string countryCode, bool isPrimary, int createdBy)
    {
        TenantId = tenantId;
        Street = street;
        City = city;
        PostalCode = postalCode;
        District = district;
        CountryCode = countryCode;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string street, string city, string postalCode, string district, string countryCode, int modifiedBy)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        District = district;
        CountryCode = countryCode;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void RemoveAsPrimary()
    {
        IsPrimary = false;
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
}
