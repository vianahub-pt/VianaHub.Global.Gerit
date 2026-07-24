namespace VianaHub.Global.Gerit.Application.Dtos.Response.Identity.UserPreferences;

/// <summary>
/// DTO de resposta detalhada para UserPreferences — inclui campos de auditoria.
/// Classe independente — não herda de UserPreferencesResponse.
/// </summary>
public class UserPreferencesDetailResponse
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Tenant { get; set; }
    public int UserId { get; set; }
    public string? User { get; set; }

    public string? Appearance { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Locale { get; set; }
    public string? Timezone { get; set; }
    public string? DateFormat { get; set; }
    public string? TimeFormat { get; set; }
    public string? DayStart { get; set; }
    public string? DayEnd { get; set; }

    public bool EmailNewsletter { get; set; }
    public bool EmailWeeklyReport { get; set; }
    public bool EmailApproval { get; set; }
    public bool EmailAlerts { get; set; }
    public bool EmailReminders { get; set; }
    public bool EmailPlanner { get; set; }

    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
