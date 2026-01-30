using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Billing;

public interface IPlanDataRepository
{
    Task<PlanEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<PlanEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<PlanEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<bool> AddAsync(PlanEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(PlanEntity entity, CancellationToken ct);
}
