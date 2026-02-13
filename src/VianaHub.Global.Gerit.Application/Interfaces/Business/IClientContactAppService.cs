using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientContact;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface de Application Service para ClientContact
/// </summary>
public interface IClientContactAppService
{
    Task<IEnumerable<ClientContactResponse>> GetAllAsync(CancellationToken ct);
    Task<ClientContactResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<ClientContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientContactRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientContactRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
