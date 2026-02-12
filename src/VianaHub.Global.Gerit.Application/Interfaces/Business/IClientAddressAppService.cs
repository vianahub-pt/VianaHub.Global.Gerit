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
    Task<IEnumerable<ClientAddressResponse>> GetAllAsync(CancellationToken ct);
    Task<ClientAddressResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ClientAddressResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientAddressRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientAddressRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
