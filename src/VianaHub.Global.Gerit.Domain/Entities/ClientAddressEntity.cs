using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa um endereço do Cliente
/// </summary>
public class ClientAddressEntity : Entity
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
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
    public ClientEntity Client { get; private set; }

    // Construtor protegido para o EF Core
    protected ClientAddressEntity() { }

    /// <summary>
    /// Construtor para criação de um novo endereço do Cliente
    /// </summary>
    public ClientAddressEntity(int tenantId, int clientId, string street, string city, string postalCode,
        string district = null, string countryCode = "PT", bool isPrimary = false)
    {
        TenantId = tenantId;
        ClientId = clientId;
        SetStreet(street);
        SetCity(city);
        SetPostalCode(postalCode);
        District = district;
        SetCountryCode(countryCode);
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Rua não pode ser vazia.", nameof(street));

        if (street.Length > 200)
            throw new ArgumentException("Rua não pode ter mais de 200 caracteres.", nameof(street));

        Street = street;
    }

    public void SetCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Cidade não pode ser vazia.", nameof(city));

        if (city.Length > 100)
            throw new ArgumentException("Cidade não pode ter mais de 100 caracteres.", nameof(city));

        City = city;
    }

    public void SetPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Código postal não pode ser vazio.", nameof(postalCode));

        if (postalCode.Length > 20)
            throw new ArgumentException("Código postal não pode ter mais de 20 caracteres.", nameof(postalCode));

        PostalCode = postalCode;
    }

    public void SetDistrict(string district)
    {
        if (district?.Length > 100)
            throw new ArgumentException("Distrito não pode ter mais de 100 caracteres.", nameof(district));

        District = district;
    }

    public void SetCountryCode(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Código do país não pode ser vazio.", nameof(countryCode));

        if (countryCode.Length != 2)
            throw new ArgumentException("Código do país deve ter exatamente 2 caracteres.", nameof(countryCode));

        CountryCode = countryCode.ToUpper();
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
