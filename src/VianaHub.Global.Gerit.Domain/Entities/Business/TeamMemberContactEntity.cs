using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um contato do Membro da Equipe
/// </summary>
public class TeamMemberContactEntity : Entity
{
    public int TenantId { get; private set; }
    public int TeamMemberId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public TeamMemberEntity TeamMember { get; private set; }

    // Construtor protegido para o EF Core
    protected TeamMemberContactEntity() { }

    /// <summary>
    /// Construtor para criação de um novo contato do Membro da Equipe
    /// </summary>
    public TeamMemberContactEntity(int tenantId, int teamMemberId, string name, string email, 
        string phone = null, bool isPrimary = false)
    {
        TenantId = tenantId;
        TeamMemberId = teamMemberId;
        SetName(name);
        SetEmail(email);
        Phone = phone;
        IsPrimary = isPrimary;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(name));

        if (name.Length > 150)
            throw new ArgumentException("Nome não pode ter mais de 150 caracteres.", nameof(name));

        Name = name;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email não pode ter mais de 255 caracteres.", nameof(email));

        Email = email;
    }

    public void SetPhone(string phone)
    {
        if (phone?.Length > 30)
            throw new ArgumentException("Telefone não pode ter mais de 30 caracteres.", nameof(phone));

        Phone = phone;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void RemoveAsPrimary()
    {
        IsPrimary = false;
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
