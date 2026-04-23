using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de servi�o de dom�nio para EmployeeAddress
/// </summary>
public interface IEmployeeAddressDomainService
{
    Task<EmployeeAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(EmployeeAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(EmployeeAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(EmployeeAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(EmployeeAddressEntity entity, CancellationToken ct);
}
