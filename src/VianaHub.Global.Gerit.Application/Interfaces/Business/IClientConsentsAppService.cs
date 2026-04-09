using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientConsents;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientConsentsAppService
{
    Task<IEnumerable<ClientConsentsResponse>> GetAllAsync(CancellationToken ct);
    Task<ClientConsentsResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ClientConsentsResponse>> GetByClientIdAsync(int clientId, CancellationToken ct);
    Task<IEnumerable<ClientConsentsResponse>> GetByConsentTypeIdAsync(int consentTypeId, CancellationToken ct);
    Task<ClientConsentsResponse> GetByClientAndConsentTypeAsync(int clientId, int consentTypeId, CancellationToken ct);
    Task<ListPageResponse<ClientConsentsResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientConsentsRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientConsentsRequest request, CancellationToken ct);
    Task<bool> RevokeConsentAsync(int id, CancellationToken ct);
    Task<bool> GrantConsentAsync(int id, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
