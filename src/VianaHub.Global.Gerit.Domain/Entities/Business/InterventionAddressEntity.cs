using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um endereço da Intervenção
/// </summary>
public class InterventionAddressEntity : Entity
{
    public int TenantId { get; private set; }
    public int InterventionId { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string District { get; private set; }
    public string CountryCode { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public InterventionEntity Intervention { get; private set; }

    // Construtor protegido para o EF Core
    protected InterventionAddressEntity() { }

    /// <summary>
    /// Construtor para criação de um novo endereço da Intervenção
    /// </summary>
    public InterventionAddressEntity(int tenantId, int interventionId, string street, string city, string postalCode,
        string district, string countryCode, bool isPrimary, int modifiedBy)
    {
        TenantId = tenantId;
        InterventionId = interventionId;
        Street = street;
        City = city;
        PostalCode = postalCode;
        District = district;
        CountryCode = countryCode;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }



    public void SetAsPrimary(int modifiedBy)
    {
        IsPrimary = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void RemoveAsPrimary(int modifiedBy)
    {
        IsPrimary = false;
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
