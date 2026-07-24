using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IEmployeeFiscalDataDomainService
{
    Task<bool> CreateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> ActivateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> DeactivateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default);
}
