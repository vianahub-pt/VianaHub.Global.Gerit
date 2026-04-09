using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do repositório de dados para VisitContact
/// </summary>
public interface IVisitContactDataRepository
{
    Task<VisitContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByVisitAndEmailAsync(int interventionId, string email, int? excludeId, CancellationToken ct);
    Task<bool> AddAsync(VisitContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitContactEntity entity, CancellationToken ct);
}
