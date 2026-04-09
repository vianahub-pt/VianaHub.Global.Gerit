using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientCompanyFiscalDataDomainService
{
    Task<bool> CreateAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientCompanyFiscalDataEntity entity, CancellationToken ct);
}
