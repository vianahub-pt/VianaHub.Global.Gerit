using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientConsentsAppService
{
    Task<IEnumerable<ClientConsentsResponse>> GetAllAsync(int clientId, CancellationToken ct);
    Task<ClientConsentsResponse> GetByIdAsync(int clientId, int id, CancellationToken ct);
    Task<ListPageResponse<ClientConsentsResponse>> GetPagedAsync(int clientId, PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(int clientId, CreateClientConsentsRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int clientId, int id, UpdateClientConsentsRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int clientId, int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int clientId, int id, CancellationToken ct);
    Task<bool> DeleteAsync(int clientId, int id, CancellationToken ct);
}
