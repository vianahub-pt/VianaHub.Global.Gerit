using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa uma Intervenção
/// Aggregate Root para o contexto de Intervenção
/// </summary>
public class InterventionEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public int StatusId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime? EndDateTime { get; private set; }
    public decimal EstimatedValue { get; private set; }
    public decimal? RealValue { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity Client { get; private set; }
    public StatusEntity Status { get; private set; }

    private readonly List<InterventionContactEntity> _contacts = new();
    public IReadOnlyCollection<InterventionContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<InterventionAddressEntity> _addresses = new();
    public IReadOnlyCollection<InterventionAddressEntity> Addresses => _addresses.AsReadOnly();

    // Construtor protegido para o EF Core
    protected InterventionEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova Intervenção
    /// </summary>
    public InterventionEntity(int tenantId, int clientId, int statusId, string title, string description, DateTime startDateTime, 
        decimal estimatedValue, int modifiedBy)
    {
        TenantId = tenantId;
        ClientId = clientId;
        StatusId = statusId;
        Title = title;
        Description = description;
        StartDateTime = startDateTime;
        EstimatedValue = estimatedValue;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = modifiedBy;
        CreatedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(int clientId, int statusId, string title, string description, DateTime startDateTime, DateTime? endDateTime,
        decimal estimatedValue, decimal? realValue, int modifiedBy)
    {
        ClientId = clientId;
        StatusId = StatusId;
        Title = title;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        EstimatedValue = estimatedValue;
        RealValue = realValue;
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

    public void AddContact(InterventionContactEntity contact)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact));

        _contacts.Add(contact);
    }

    public void AddAddress(InterventionAddressEntity address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        _addresses.Add(address);
    }
}
