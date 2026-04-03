using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientTypeDataRepository
{
    Task<IEnumerable<ClientTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ClientTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<ClientTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);

    Task<bool> AddAsync(ClientTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientTypeEntity entity, CancellationToken ct);
}
