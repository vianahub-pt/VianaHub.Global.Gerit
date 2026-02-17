using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do repositório de dados para InterventionContact
/// </summary>
public interface IInterventionContactDataRepository
{
    Task<InterventionContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<InterventionContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<InterventionContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByInterventionAndEmailAsync(int interventionId, string email, int? excludeId, CancellationToken ct);
    Task<bool> AddAsync(InterventionContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(InterventionContactEntity entity, CancellationToken ct);
}
