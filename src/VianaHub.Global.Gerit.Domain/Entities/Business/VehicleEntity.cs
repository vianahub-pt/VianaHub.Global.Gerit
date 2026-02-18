using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Veículo
/// </summary>
public class VehicleEntity : Entity
{
    public int TenantId { get; private set; }
    public int StatusId { get; private set; }
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
    public StatusEntity Status { get; private set; }

    // Construtor protegido para o EF Core
    protected VehicleEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Veículo
    /// </summary>
    public VehicleEntity(int tenantId, int statusId, string plate, string brand, string model, int year, string color, string fuelType, int createdBy)
    {
        TenantId = tenantId;
        StatusId = statusId;
        Plate = plate;
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
        FuelType = fuelType;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int statusId, string plate, string brand, string model, int year, string color, string fuelType, int modifiedBy)
    {
        StatusId = statusId;
        Plate = plate;
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
        FuelType = fuelType;
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
