using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IConsentTypeDataRepository
{
    Task<IEnumerable<ConsentTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ConsentTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<ConsentTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);

    Task<bool> AddAsync(ConsentTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ConsentTypeEntity entity, CancellationToken ct);
}
