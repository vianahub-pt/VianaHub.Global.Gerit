using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Enums;

namespace VianaHub.Global.Gerit.Domain.Entities.Business;

/// <summary>
/// Entidade que representa uma Intervenção
/// Aggregate Root para o contexto de Intervenção
/// </summary>
public class InterventionEntity : Entity, IAggregateRoot
{
    public int TenantId { get; private set; }
    public int ClientId { get; private set; }
    public int TeamMemberId { get; private set; }
    public int VehicleId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime? EndDateTime { get; private set; }
    public decimal EstimatedValue { get; private set; }
    public decimal? RealValue { get; private set; }
    public InterventionStatus Status { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public TenantEntity Tenant { get; private set; }
    public ClientEntity Client { get; private set; }
    public TeamMemberEntity TeamMember { get; private set; }
    public VehicleEntity Vehicle { get; private set; }

    private readonly List<InterventionContactEntity> _contacts = new();
    public IReadOnlyCollection<InterventionContactEntity> Contacts => _contacts.AsReadOnly();

    private readonly List<InterventionAddressEntity> _addresses = new();
    public IReadOnlyCollection<InterventionAddressEntity> Addresses => _addresses.AsReadOnly();

    // Construtor protegido para o EF Core
    protected InterventionEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova Intervenção
    /// </summary>
    public InterventionEntity(int tenantId, int clientId, int teamMemberId, int vehicleId,
        string title, string description, DateTime startDateTime, decimal estimatedValue, int modifiedBy)
    {
        TenantId = tenantId;
        ClientId = clientId;
        TeamMemberId = teamMemberId;
        VehicleId = vehicleId;
        Title = title;
        Description = description;
        StartDateTime = startDateTime;
        EstimatedValue = estimatedValue;
        Status = InterventionStatus.Pending;
        IsActive = true;
        IsDeleted = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description, DateTime startDateTime, decimal estimatedValue, int modifiedBy)
    {
        Title = title;
        Description = description;
        StartDateTime = startDateTime;
        EstimatedValue = estimatedValue;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetStatus(InterventionStatus status, int modifiedBy)
    {
        Status = status;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Start(int modifiedBy)
    {
        Status = InterventionStatus.InProgress;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Pause(int modifiedBy)
    {
        Status = InterventionStatus.Paused;
    }

    public void Complete(decimal realValue, int modifiedBy)
    {
        RealValue = realValue;
        EndDateTime = DateTime.UtcNow;
        Status = InterventionStatus.Completed;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Cancel(int modifiedBy)
    {
        Status = InterventionStatus.Cancelled;
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
