using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de serviço de domínio para ClientAddress
/// </summary>
public interface IClientAddressDomainService
{
    Task<ClientAddressEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ClientAddressEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<ClientAddressEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);

    Task<bool> CreateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientAddressEntity entity, CancellationToken ct);
}
