using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa um Equipamento
/// </summary>
public class EquipmentEntity : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string SerialNumber { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected EquipmentEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Equipamento
    /// </summary>
    public EquipmentEntity(int tenantId, string name, string serialNumber = null)
    {
        TenantId = tenantId;
        SetName(name);
        SerialNumber = serialNumber;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do equipamento não pode ser vazio.", nameof(name));

        if (name.Length > 150)
            throw new ArgumentException("Nome do equipamento não pode ter mais de 150 caracteres.", nameof(name));

        Name = name;
    }

    public void SetSerialNumber(string serialNumber)
    {
        if (serialNumber?.Length > 100)
            throw new ArgumentException("Número de série não pode ter mais de 100 caracteres.", nameof(serialNumber));

        SerialNumber = serialNumber;
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
