using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de reposit�rio de dados para EmployeeAddress
/// </summary>
public interface IEmployeeAddressDataRepository
{
    Task<EmployeeAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<EmployeeAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<EmployeeAddressEntity>> GetPagedAsync(PagedFilter filter, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByEmployeeAndAddressAsync(int tenantId, int EmployeeId, string street, string city, string postalCode, CancellationToken ct);
    Task<EmployeeAddressEntity> GetPrimaryAddressByEmployeeAsync(int EmployeeId, CancellationToken ct);
    Task<bool> AddAsync(EmployeeAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EmployeeAddressEntity entity, CancellationToken ct);
}
