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
    public int FunctionId { get; private set; }
    public string Name { get; private set; }
    public string TaxNumber { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public FunctionEntity Function { get; private set; }

    private readonly List<TeamMemberContactEntity> _contacts = new();
    public IReadOnlyCollection<TeamMemberContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<TeamMemberAddressEntity> _addresses = new();
    public IReadOnlyCollection<TeamMemberAddressEntity> Addresses => _addresses.AsReadOnly();

    // Construtor protegido para o EF Core
    protected TeamMemberEntity() { }

    /// <summary>
    /// Construtor para criação de um novo Membro da Equipe
    /// </summary>
    public TeamMemberEntity(int tenantId, int functionId, string name, string taxNumber, int createdBy)
    {
        TenantId = tenantId;
        FunctionId = functionId;
        Name = name;
        TaxNumber = taxNumber;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int functionId, string name, int modifiedBy)
    {
        FunctionId = functionId;
        Name = name;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateTaxNumber(string taxNumber, int modifiedBy)
    {
        TaxNumber = taxNumber;
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
