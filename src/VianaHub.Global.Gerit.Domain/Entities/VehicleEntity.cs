using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa um Veículo
/// </summary>
public class VehicleEntity : Entity
{
    public int TenantId { get; private set; }
    public string Plate { get; private set; }
    public string Model { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected VehicleEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Veículo
    /// </summary>
    public VehicleEntity(int tenantId, string plate, string model = null)
    {
        TenantId = tenantId;
        SetPlate(plate);
        Model = model;
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

    public void SetModel(string model)
    {
        if (model?.Length > 100)
            throw new ArgumentException("Modelo não pode ter mais de 100 caracteres.", nameof(model));

        Model = model;
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
