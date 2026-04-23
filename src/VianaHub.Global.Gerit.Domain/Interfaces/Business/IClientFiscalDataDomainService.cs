using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientFiscalDataDomainService
{
    Task<bool> CreateAsync(ClientFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(ClientFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> ActivateAsync(ClientFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> DeactivateAsync(ClientFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(ClientFiscalDataEntity entity, CancellationToken ct = default);
}
