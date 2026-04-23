using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientIndividual;
using VianaHub.Global.Gerit.Application.Dtos.Response.Business.ClientIndividual;

namespace VianaHub.Global.Gerit.Application.Interfaces.Business;

public interface IClientIndividualAppService
{
    Task<ClientIndividualResponse> GetByIdAsync(int id, CancellationToken ct);
    Task<ClientIndividualResponse> GetByClientIdAsync(int clientId, CancellationToken ct);
    Task<ClientIndividualResponse> GetByDocumentAsync(string documentType, string documentNumber, CancellationToken ct);
    Task<IEnumerable<ClientIndividualResponse>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<ClientIndividualResponse>> GetActiveAsync(CancellationToken ct);
    Task<ListPageResponse<ClientIndividualResponse>> GetPagedAsync(PagedFilterRequest request, CancellationToken ct);
    Task<bool> CreateAsync(CreateClientIndividualRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateClientIndividualRequest request, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
