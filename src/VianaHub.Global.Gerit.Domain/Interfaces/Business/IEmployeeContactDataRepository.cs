using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de reposit�rio para EmployeeContact
/// </summary>
public interface IEmployeeContactDataRepository
{
    Task<EmployeeContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByEmailAsync(int tenantId, int EmployeeId, string email, CancellationToken ct);
    Task<bool> ExistsByEmailForUpdateAsync(int tenantId, int EmployeeId, string email, int excludeId, CancellationToken ct);
    Task<bool> AddAsync(EmployeeContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeContactEntity entity, CancellationToken ct);
}
