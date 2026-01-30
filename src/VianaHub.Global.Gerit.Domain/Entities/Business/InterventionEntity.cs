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
        string title, string description, DateTime startDateTime, decimal estimatedValue)
    {
        TenantId = tenantId;
        ClientId = clientId;
        TeamMemberId = teamMemberId;
        VehicleId = vehicleId;
        SetTitle(title);
        SetDescription(description);
        SetStartDateTime(startDateTime);
        SetEstimatedValue(estimatedValue);
        Status = InterventionStatus.Pending;
        IsActive = true;
        IsDeleted = false;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título não pode ser vazio.", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Título não pode ter mais de 200 caracteres.", nameof(title));

        Title = title;
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(description));

        if (description.Length > 2000)
            throw new ArgumentException("Descrição não pode ter mais de 2000 caracteres.", nameof(description));

        Description = description;
    }

    public void SetStartDateTime(DateTime startDateTime)
    {
        if (startDateTime == default)
            throw new ArgumentException("Data de início inválida.", nameof(startDateTime));

        StartDateTime = startDateTime;
    }

    public void SetEndDateTime(DateTime? endDateTime)
    {
        if (endDateTime.HasValue && endDateTime.Value < StartDateTime)
            throw new ArgumentException("Data de fim não pode ser anterior à data de início.", nameof(endDateTime));

        EndDateTime = endDateTime;
    }

    public void SetEstimatedValue(decimal estimatedValue)
    {
        if (estimatedValue < 0)
            throw new ArgumentException("Valor estimado não pode ser negativo.", nameof(estimatedValue));

        EstimatedValue = estimatedValue;
    }

    public void SetRealValue(decimal? realValue)
    {
        if (realValue.HasValue && realValue.Value < 0)
            throw new ArgumentException("Valor real não pode ser negativo.", nameof(realValue));

        RealValue = realValue;
    }

    public void SetStatus(InterventionStatus status)
    {
        Status = status;
    }

    public void Start()
    {
        if (Status != InterventionStatus.Pending && Status != InterventionStatus.Paused)
            throw new InvalidOperationException("Intervenção só pode ser iniciada se estiver pendente ou pausada.");

        Status = InterventionStatus.InProgress;
    }

    public void Pause()
    {
        if (Status != InterventionStatus.InProgress)
            throw new InvalidOperationException("Intervenção só pode ser pausada se estiver em progresso.");

        Status = InterventionStatus.Paused;
    }

    public void Complete(decimal realValue)
    {
        if (Status != InterventionStatus.InProgress)
            throw new InvalidOperationException("Intervenção só pode ser concluída se estiver em progresso.");

        SetRealValue(realValue);
        EndDateTime = DateTime.UtcNow;
        Status = InterventionStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == InterventionStatus.Completed)
            throw new InvalidOperationException("Intervenção já concluída não pode ser cancelada.");

        Status = InterventionStatus.Cancelled;
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
