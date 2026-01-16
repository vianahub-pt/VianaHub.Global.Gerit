using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa um recurso do sistema
/// </summary>
public class ResourceEntity : Entity
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    private readonly List<RolePermissionEntity> _rolePermissions = new();
    public IReadOnlyCollection<RolePermissionEntity> RolePermissions => _rolePermissions.AsReadOnly();

    // Construtor protegido para o EF Core
    protected ResourceEntity() { }

    /// <summary>
    /// Construtor para criação de um novo recurso
    /// </summary>
    public ResourceEntity(string name)
    {
        SetName(name);
        IsActive = true;
        IsDeleted = false;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do recurso não pode ser vazio.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Nome do recurso não pode ter mais de 100 caracteres.", nameof(name));

        Name = name;
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
