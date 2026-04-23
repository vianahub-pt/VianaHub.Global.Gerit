using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do serviço de domínio para VisitContact
/// </summary>
public interface IVisitContactDomainService
{
    Task<VisitContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(VisitContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(VisitContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(VisitContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(VisitContactEntity entity, CancellationToken ct);
}
