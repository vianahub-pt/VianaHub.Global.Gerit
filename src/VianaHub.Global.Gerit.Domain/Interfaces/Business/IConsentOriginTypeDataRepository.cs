using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IConsentOriginTypeDataRepository
{
    Task<IEnumerable<ConsentOriginTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ConsentOriginTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPage<ConsentOriginTypeEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);

    Task<bool> AddAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ConsentOriginTypeEntity entity, CancellationToken ct);
}
