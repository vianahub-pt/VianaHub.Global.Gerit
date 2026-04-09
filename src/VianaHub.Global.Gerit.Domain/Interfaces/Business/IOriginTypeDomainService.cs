using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IOriginTypeDomainService
{
    Task<OriginTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<OriginTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<OriginTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateAsync(OriginTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(OriginTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(OriginTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(OriginTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(OriginTypeEntity entity, CancellationToken ct);
}
