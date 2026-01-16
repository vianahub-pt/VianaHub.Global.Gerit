using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa uma Role (papel) no sistema RBAC
/// </summary>
public class RoleEntity : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    private readonly List<RolePermissionEntity> _permissions = new();
    public IReadOnlyCollection<RolePermissionEntity> Permissions => _permissions.AsReadOnly();

    private readonly List<UserRoleEntity> _userRoles = new();
    public IReadOnlyCollection<UserRoleEntity> UserRoles => _userRoles.AsReadOnly();

    // Construtor protegido para o EF Core
    protected RoleEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova Role
    /// </summary>
    public RoleEntity(int tenantId, string name)
    {
        TenantId = tenantId;
        SetName(name);
        IsActive = true;
        IsDeleted = false;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da role não pode ser vazio.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Nome da role não pode ter mais de 100 caracteres.", nameof(name));

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

    public void AddPermission(RolePermissionEntity permission)
    {
        if (permission == null)
            throw new ArgumentNullException(nameof(permission));

        if (_permissions.Any(p => p.ResourceId == permission.ResourceId && p.ActionId == permission.ActionId))
            throw new InvalidOperationException("Esta permissão já existe para esta role.");

        _permissions.Add(permission);
    }

    public void RemovePermission(int resourceId, int actionId)
    {
        var permission = _permissions.FirstOrDefault(p => p.ResourceId == resourceId && p.ActionId == actionId);
        if (permission != null)
            _permissions.Remove(permission);
    }
}
