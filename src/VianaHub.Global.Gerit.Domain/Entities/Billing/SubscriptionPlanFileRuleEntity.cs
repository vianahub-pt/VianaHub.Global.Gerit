using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Entities.Billing;

/// <summary>
/// Entidade que representa uma regra de arquivo permitida para um SubscriptionPlan.
/// Define o tamanho máximo (em MB) para cada FileType suportado por um plano.
/// Tabela: dbo.SubscriptionPlanFileRules
/// </summary>
public class SubscriptionPlanFileRuleEntity : Entity
{
    public int SubscriptionPlanId { get; private set; }
    public int FileTypeId { get; private set; }
    public int MaxFileSizeMB { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public SubscriptionPlanEntity SubscriptionPlan { get; private set; } = null!;
    public FileTypeEntity FileType { get; private set; } = null!;

    // Construtor protegido para o EF Core
    protected SubscriptionPlanFileRuleEntity() { }

    /// <summary>
    /// Construtor para criação de uma nova regra de arquivo para SubscriptionPlan
    /// </summary>
    public SubscriptionPlanFileRuleEntity(int subscriptionPlanId, int fileTypeId, int maxFileSizeMB, int createdBy)
    {
        SubscriptionPlanId = subscriptionPlanId;
        FileTypeId = fileTypeId;
        MaxFileSizeMB = maxFileSizeMB;
        IsActive = true;
        IsDeleted = false;

        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(int fileTypeId, int maxFileSizeMB, int modifiedBy)
    {
        FileTypeId = fileTypeId;
        MaxFileSizeMB = maxFileSizeMB;
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
        IsActive = false;
        IsDeleted = true;
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
