using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface para serviço de domínio de Status
/// </summary>
public interface IStatusDomainService
{
    Task<StatusEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<StatusEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<StatusEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(StatusEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(StatusEntity entity, CancellationToken ct);
}
