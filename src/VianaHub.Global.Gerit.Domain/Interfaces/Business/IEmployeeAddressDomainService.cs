using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de servi�o de dom�nio para EmployeeAddress
/// </summary>
public interface IEmployeeAddressDomainService
{
    Task<EmployeeAddressesEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeAddressesEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeAddressesEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(EmployeeAddressesEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeAddressesEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(EmployeeAddressesEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(EmployeeAddressesEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(EmployeeAddressesEntity entity, CancellationToken ct);
}
