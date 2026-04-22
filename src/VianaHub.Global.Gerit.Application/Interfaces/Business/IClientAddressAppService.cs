using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientAddress;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientAddress;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de serviço de aplicação para ClientAddress
/// </summary>
public interface IClientAddressAppService
{
    Task<ClientAddressResponse> GetByIdAsync(int clientId, int id, CancellationToken ct);
    Task<IEnumerable<ClientAddressResponse>> GetAllAsync(int clientId, CancellationToken ct);
    Task<ListPageResponse<ClientAddressResponse>> GetPagedAsync(int clientId, PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(int clientId, CreateClientAddressRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int clientId, int id, UpdateClientAddressRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int clientId, int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int clientId, int id, CancellationToken ct);
    Task<bool> DeleteAsync(int clientId, int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(int clientId, IFormFile file, CancellationToken ct);
}
