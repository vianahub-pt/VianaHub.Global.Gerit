using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

public class FunctionEntity : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Property
    public TenantEntity Tenant { get; private set; }

    // Construtor protegido para o EF Core
    protected FunctionEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Equipamento
    /// </summary>
    public FunctionEntity(int tenantId, string name, string description)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        IsActive = true;
        IsDeleted = false;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
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

