using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Interfaces.Business;

public  interface IFunctionDomainService
{
    Task<FunctionEntity> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<FunctionEntity>> GetAllAsync(CancellationToken ct);
    Task<ListPage<FunctionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateAsync(FunctionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(FunctionEntity entity, CancellationToken ct);
    Task<bool> ActivateAsync(FunctionEntity entity, CancellationToken ct);
    Task<bool> DeactivateAsync(FunctionEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(FunctionEntity entity, CancellationToken ct);
}
