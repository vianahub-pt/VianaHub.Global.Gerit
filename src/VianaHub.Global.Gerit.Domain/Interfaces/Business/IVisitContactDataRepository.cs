using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface do reposit�rio de dados para VisitContact
/// </summary>
public interface IVisitContactDataRepository
{
    Task<VisitContactPersonsEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<VisitContactPersonsEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<VisitContactPersonsEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByVisitAndEmailAsync(int interventionId, string email, int? excludeId, CancellationToken ct);
    Task<bool> AddAsync(VisitContactPersonsEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(VisitContactPersonsEntity entity, CancellationToken ct);
}
