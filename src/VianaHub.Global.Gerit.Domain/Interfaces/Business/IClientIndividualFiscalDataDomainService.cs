using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientIndividualFiscalDataDomainService
{
    Task<bool> CreateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> ActivateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> DeactivateAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(ClientIndividualFiscalDataEntity entity, CancellationToken ct = default);
}
