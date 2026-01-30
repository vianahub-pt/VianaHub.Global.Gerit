using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa um Membro da Equipe
/// Aggregate Root para o contexto de TeamMember
/// </summary>
public class TeamMemberEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public string Name { get; private set; }
    public string TaxNumber { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }

    private readonly List<TeamMemberContactEntity> _contacts = new();
    public IReadOnlyCollection<TeamMemberContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<TeamMemberAddressEntity> _addresses = new();
    public IReadOnlyCollection<TeamMemberAddressEntity> Addresses => _addresses.AsReadOnly();

    // Construtor protegido para o EF Core
    protected TeamMemberEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Membro da Equipe
    /// </summary>
    public TeamMemberEntity(int tenantId, string name, string taxNumber = null)
    {
        TenantId = tenantId;
        SetName(name);
        TaxNumber = taxNumber;
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

    public void SetTaxNumber(string taxNumber)
    {
        if (taxNumber?.Length > 20)
            throw new ArgumentException("Número fiscal não pode ter mais de 20 caracteres.", nameof(taxNumber));

        TaxNumber = taxNumber;
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

    public void AddContact(TeamMemberContactEntity contact)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact));

        _contacts.Add(contact);
    }

    public void AddAddress(TeamMemberAddressEntity address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _addresses.Add(address);
    }
}
