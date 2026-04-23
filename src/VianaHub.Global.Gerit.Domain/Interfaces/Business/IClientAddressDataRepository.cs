using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

/// <summary>
/// Interface de repositório para ClientAddress
/// </summary>
public interface IClientAddressDataRepository
{
    Task<ClientAddressEntity> GetByIdAsync(int clientId, int id, CancellationToken ct);
    Task<IEnumerable<ClientAddressEntity>> GetAllAsync(int clientId, CancellationToken ct);
    Task<ListPage<ClientAddressEntity>> GetPagedAsync(int clientId, PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsByClientIdAsync(int clientId, string countryCode, string street, string streetNumber, string neighborhood, string city, string district, string postalCode, CancellationToken ct);
    Task<bool> ExistsByClientAndAddressTypeAsync(int clientId, int addressTypeId, CancellationToken ct);
    Task<bool> CreateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClientAddressEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(ClientAddressEntity entity, CancellationToken ct);
}
