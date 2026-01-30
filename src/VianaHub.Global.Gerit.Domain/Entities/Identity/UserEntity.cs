using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Identity;

/// <summary>
/// Entidade que representa um usuário do sistema
/// </summary>
public class UserEntity : Entity
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string NormalizedEmail { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }
    public DateTime? LastAccessAt { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    private readonly List<UserRoleEntity> _userRoles = new();
    public IReadOnlyCollection<UserRoleEntity> UserRoles => _userRoles.AsReadOnly();

    // Construtor protegido para o EF Core
    protected UserEntity() { }

    /// <summary>
    /// Construtor para criação de um novo usuário
    /// </summary>
    public UserEntity(int tenantId, string name, string email, string passwordHash, string phoneNumber, int createdBy)
    {
        TenantId = tenantId;
        Name = name;
        Email = email;
        NormalizedEmail = email?.ToUpperInvariant();
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        EmailConfirmed = false;
        PhoneNumberConfirmed = false;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
    }

    public void Update(string name, string phoneNumber, int modifiedBy)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash, int modifiedBy)
    {
        PasswordHash = passwordHash;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void ConfirmEmail(int modifiedBy)
    {
        EmailConfirmed = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void ConfirmPhoneNumber(int modifiedBy)
    {
        PhoneNumberConfirmed = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateLastAccess()
    {
        LastAccessAt = DateTime.UtcNow;
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

    public void AddRole(UserRoleEntity userRole)
    {
        if (userRole == null)
            throw new ArgumentNullException(nameof(userRole));

        if (_userRoles.Any(ur => ur.RoleId == userRole.RoleId))
            throw new InvalidOperationException("Usuário já possui esta role.");

        _userRoles.Add(userRole);
    }

    public void RemoveRole(int roleId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
        if (userRole != null)
            _userRoles.Remove(userRole);
    }
}
