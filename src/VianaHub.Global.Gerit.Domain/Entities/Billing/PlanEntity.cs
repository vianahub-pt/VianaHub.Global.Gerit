using VianaHub.Global.Gerit.Domain.Base;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa um plano de assinatura do sistema
/// </summary>
public class PlanEntity : Entity
{
    public string Name { get; private set; }
    public string Description { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? PricePerDay { get; set; }
    public decimal? PricePerMonth { get; set; }
    public decimal? PricePerYear { get; set; }
    public string Currency { get; private set; }
    public int MaxUsers { get; private set; }
    public int MaxPhotosPerVisits { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    private readonly List<SubscriptionEntity> _subscriptions = new();
    public IReadOnlyCollection<SubscriptionEntity> Subscriptions => _subscriptions.AsReadOnly();

    // Construtor protegido para o EF Core
    protected PlanEntity() { }

    /// <summary>
    /// Construtor para criação de um novo plano
    /// </summary>
    public PlanEntity(
        string name, 
        string description, 
        decimal? pricePerHour,
        decimal? pricePerDay,
        decimal? pricePerMonth,
        decimal? pricePerYear,
        string currency,
        int maxUsers,
        int maxPhotosPerVisits,
        int createdBy)
    {
        Name = name;
        Description = description;
        PricePerHour = pricePerHour;
        PricePerDay = pricePerDay;
        PricePerMonth = pricePerMonth;
        PricePerYear = pricePerYear;
        Currency = currency ?? "USD";
        MaxUsers = maxUsers;
        MaxPhotosPerVisits = maxPhotosPerVisits;
        IsActive = true;
        IsDeleted = false;
        CreatedBy = createdBy;
    }

    public void Update(
        string name, 
        string description,
        decimal? pricePerHour,
        decimal? pricePerDay,
        decimal? pricePerMonth,
        decimal? pricePerYear,
        string currency,
        int maxUsers,
        int maxPhotosPerVisits,
        int modifiedBy)
    {
        Name = name;
        Description = description;
        PricePerHour = pricePerHour;
        PricePerDay = pricePerDay;
        PricePerMonth = pricePerMonth;
        PricePerYear = pricePerYear;
        Currency = currency ?? "USD";
        MaxUsers = maxUsers;
        MaxPhotosPerVisits = maxPhotosPerVisits;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
    
    public void Activate(int? modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate(int? modifiedBy)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Delete(int? modifiedBy)
    {
        IsDeleted = true;
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
