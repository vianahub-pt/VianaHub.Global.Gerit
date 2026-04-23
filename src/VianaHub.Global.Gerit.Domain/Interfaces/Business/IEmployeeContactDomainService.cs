using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de servi�o de dom�nio para EmployeeContact
/// </summary>
public interface IEmployeeContactDomainService
{
    Task<EmployeeContactEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeContactEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeContactEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateAsync(EmployeeContactEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeContactEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(EmployeeContactEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(EmployeeContactEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(EmployeeContactEntity entity, CancellationToken ct);
}
