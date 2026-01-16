using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Repository;

public interface IActionDataRepository
{
    Task<ActionEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ActionEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<ActionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<bool> AddAsync(ActionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ActionEntity entity, CancellationToken ct);
}
