using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientHierarchy;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientHierarchyAppService
{
    Task<IEnumerable<ClientHierarchyResponse>> GetAllAsync(CancellationToken ct);
    Task<ClientHierarchyResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<ClientHierarchyResponse>> GetByParentClientIdAsync(int parentClientId, CancellationToken ct);
    Task<IEnumerable<ClientHierarchyResponse>> GetByChildClientIdAsync(int childClientId, CancellationToken ct);
    Task<ListPageResponse<ClientHierarchyResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientHierarchyRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientHierarchyRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
