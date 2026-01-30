using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Veículo
/// </summary>
public class VehicleEntity : Entity
{
    public int TenantId { get; private set; }
    public string Plate { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public string Color { get; private set; }
    public string FuelType { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected VehicleEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Veículo
    /// </summary>
    public VehicleEntity(int tenantId, string plate, string brand, string model, int year, string color = null, string fuelType = null)
    {
        TenantId = tenantId;
        SetPlate(plate);
        SetBrand(brand);
        SetModel(model);
        SetYear(year);
        SetColor(color);
        SetFuelType(fuelType);
        IsActive = true;
        IsDeleted = false;
    }

    public void SetPlate(string plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
            throw new ArgumentException("Placa não pode ser vazia.", nameof(plate));

        if (plate.Length > 20)
            throw new ArgumentException("Placa não pode ter mais de 20 caracteres.", nameof(plate));

        Plate = plate.ToUpper();
    }

    public void SetBrand(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Marca não pode ser vazia.", nameof(brand));

        if (brand.Length > 100)
            throw new ArgumentException("Marca não pode ter mais de 100 caracteres.", nameof(brand));

        Brand = brand;
    }

    public void SetModel(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Modelo não pode ser vazio.", nameof(model));

        if (model.Length > 100)
            throw new ArgumentException("Modelo não pode ter mais de 100 caracteres.", nameof(model));

        Model = model;
    }

    public void SetYear(int year)
    {
        var currentYear = DateTime.UtcNow.Year + 1; // accept next year optionally
        if (year < 1886 || year > currentYear)
            throw new ArgumentException($"Ano inválido. Deve estar entre 1886 e {currentYear}.", nameof(year));

        Year = year;
    }

    public void SetColor(string color)
    {
        if (color != null && color.Length > 50)
            throw new ArgumentException("Cor não pode ter mais de 50 caracteres.", nameof(color));

        Color = color;
    }

    public void SetFuelType(string fuelType)
    {
        if (fuelType != null && fuelType.Length > 50)
            throw new ArgumentException("Tipo de combustível não pode ter mais de 50 caracteres.", nameof(fuelType));

        FuelType = fuelType;
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
