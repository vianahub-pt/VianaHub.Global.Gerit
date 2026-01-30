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
        Plate = plate;
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
        FuelType = fuelType;
        IsActive = true;
        IsDeleted = false;
    }

    public void Update(string plate, string brand, string model, int year, string color = null, string fuelType = null)
    {
        Plate = plate;
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
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
