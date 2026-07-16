using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface ISubscriptionPlanFileRuleDataRepository
{
    Task<IEnumerable<SubscriptionPlanFileRuleEntity>> GetAllAsync(CancellationToken ct);
    Task<SubscriptionPlanFileRuleEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<SubscriptionPlanFileRuleEntity>> GetBySubscriptionPlanIdAsync(int subscriptionPlanId, CancellationToken ct);
    Task<ListPage<SubscriptionPlanFileRuleEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> AddAsync(SubscriptionPlanFileRuleEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(SubscriptionPlanFileRuleEntity entity, CancellationToken ct);
}
