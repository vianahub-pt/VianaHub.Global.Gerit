using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um endereço do Cliente
/// </summary>
public class ClientAddressEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public int AddressTypeId { get; private set; }
    public string CountryCode { get; private set; }
    public string Street { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string District { get; private set; }
    public string PostalCode { get; private set; }
    public string StreetNumber { get; private set; }
    public string Complement { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string Notes { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity Client { get; private set; }
    public AddressTypeEntity AddressType { get; private set; }

    // Construtor protegido para o EF Core
    protected ClientAddressEntity() { }

    /// <summary>
    /// Construtor para criação de um novo endereço do Cliente
    /// </summary>
    public ClientAddressEntity(
        int tenantId, 
        int clientId, 
        int addressTypeId,
        string countryCode,
        string street,
        string neighborhood,
        string city, 
        string district,
        string postalCode,
        string streetNumber,
        string complement,
        decimal? latitude,
        decimal? longitude,
        string notes,
        bool isPrimary, 
        int createdBy)
    {
        TenantId = tenantId;
        ClientId = clientId;
        AddressTypeId = addressTypeId;
        CountryCode = countryCode ?? "PT";
        Street  = street;
        Neighborhood = neighborhood;
        City = city;
        District = district;
        PostalCode = postalCode;
        StreetNumber = streetNumber;
        Complement = complement;
        Latitude = latitude;
        Longitude = longitude;
        Notes = notes;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(
        int addressTypeId,
        string countryCode,
        string street,
        string neighborhood,
        string city, 
        string district,
        string postalCode, 
        string streetNumber,
        string complement,
        decimal? latitude,
        decimal? longitude,
        string notes,
        int modifiedBy)
    {
        AddressTypeId = addressTypeId;
        CountryCode = countryCode ?? "PT";
        Street = street;
        Neighborhood = neighborhood;
        City = city;
        District = district;
        PostalCode = postalCode;
        StreetNumber = streetNumber;
        Complement = complement;
        Latitude = latitude;
        Longitude = longitude;
        Notes = notes;
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
