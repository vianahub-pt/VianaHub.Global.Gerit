using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.InterventionContact;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

/// <summary>
/// Interface do serviço de aplicação para InterventionContact
/// </summary>
public interface IInterventionContactAppService
{
    Task<IEnumerable<InterventionContactResponse>> GetAllAsync(CancellationToken ct);
    Task<InterventionContactResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ListPageResponse<InterventionContactResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateInterventionContactRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateInterventionContactRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<bool> BulkUploadAsync(IFormFile file, CancellationToken ct);
}
