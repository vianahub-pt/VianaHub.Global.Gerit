using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de servi�o de dom�nio para EmployeeContact
/// </summary>
public interface IEmployeeContactDomainService
{
    Task<EmployeeContactPersonsEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeContactPersonsEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeContactPersonsEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateAsync(EmployeeContactPersonsEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeContactPersonsEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(EmployeeContactPersonsEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(EmployeeContactPersonsEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(EmployeeContactPersonsEntity entity, CancellationToken ct);
}
