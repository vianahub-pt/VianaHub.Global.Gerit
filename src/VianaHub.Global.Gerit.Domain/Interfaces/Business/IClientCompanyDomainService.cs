using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public interface IClientCompanyDomainService
{
    Task<bool> CreateAsync(ClientCompanyEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientCompanyEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientCompanyEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientCompanyEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientCompanyEntity entity, CancellationToken ct);
}
