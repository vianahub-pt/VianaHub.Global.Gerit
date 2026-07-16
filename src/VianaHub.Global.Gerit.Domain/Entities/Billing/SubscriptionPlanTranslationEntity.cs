namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade de tradução de um SubscriptionPlan por idioma (pt-PT, en-US, es-ES).
/// Chave composta: (SubscriptionPlanId + LanguageCode).
/// </summary>
public class SubscriptionPlanTranslationEntity
{
    public int SubscriptionPlanId { get; private set; }
    public string? LanguageCode { get; private set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    // Navigation Property
    public SubscriptionPlanEntity SubscriptionPlan { get; private set; }

    // Construtor protegido para o EF Core
    protected SubscriptionPlanTranslationEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova tradução de SubscriptionPlan
    /// </summary>
    public SubscriptionPlanTranslationEntity(int subscriptionPlanId, string languageCode, string name, string description)
    {
        SubscriptionPlanId = subscriptionPlanId;
        LanguageCode = languageCode;
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
