using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IFunctionDataRepository
{
    Task<FunctionEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<FunctionEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<FunctionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int tenantId, string name, CancellationToken ct);
    Task<bool> AddAsync(FunctionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(FunctionEntity entity, CancellationToken ct);
}
