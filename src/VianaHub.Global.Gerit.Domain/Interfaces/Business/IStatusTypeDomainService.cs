using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IStatusTypeDomainService
{
    Task<StatusTypeEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<StatusTypeEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<StatusTypeEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(StatusTypeEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(StatusTypeEntity entity, CancellationToken ct);
}
