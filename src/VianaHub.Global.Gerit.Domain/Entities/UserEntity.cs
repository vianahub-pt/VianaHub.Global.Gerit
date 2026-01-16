using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities;

/// <summary>
/// Entidade que representa um usuário do sistema
/// </summary>
public class UserEntity : Entity
{
    public int TenantId { get; private set; }
    public string Email { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public string FullName { get; private set; }
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
    public UserEntity(int tenantId, string email, byte[] passwordHash, string fullName)
    {
        TenantId = tenantId;
        SetEmail(email);
        SetPasswordHash(passwordHash);
        SetFullName(fullName);
        IsActive = true;
        IsDeleted = false;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email não pode ter mais de 255 caracteres.", nameof(email));

        if (!email.Contains("@"))
            throw new ArgumentException("Email inválido.", nameof(email));

        Email = email.ToLower();
    }

    public void SetPasswordHash(byte[] passwordHash)
    {
        if (passwordHash == null || passwordHash.Length == 0)
            throw new ArgumentException("Hash da senha não pode ser vazio.", nameof(passwordHash));

        if (passwordHash.Length != 64)
            throw new ArgumentException("Hash da senha deve ter 64 bytes.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void SetFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Nome completo não pode ser vazio.", nameof(fullName));

        if (fullName.Length > 150)
            throw new ArgumentException("Nome completo não pode ter mais de 150 caracteres.", nameof(fullName));

        FullName = fullName;
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
