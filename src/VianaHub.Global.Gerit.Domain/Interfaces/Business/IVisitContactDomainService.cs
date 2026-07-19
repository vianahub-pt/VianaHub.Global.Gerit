using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do servi�o de dom�nio para VisitContact
/// </summary>
public interface IVisitContactDomainService
{
    Task<VisitContactPersonsEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitContactPersonsEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitContactPersonsEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(VisitContactPersonsEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitContactPersonsEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(VisitContactPersonsEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(VisitContactPersonsEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(VisitContactPersonsEntity entity, CancellationToken ct);
}
