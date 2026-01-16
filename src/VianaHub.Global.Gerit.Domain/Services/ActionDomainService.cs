using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Services;

public class ActionDomainService : IActionDomainService
{
    private readonly IActionDataRepository _repo;

    public ActionDomainService(IActionDataRepository repo)
    {
        _repo = repo;
    }

    public async Task<ActionEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.GetByIdAsync(id, ct);
    }
    public async Task<IEnumerable<ActionEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _repo.GetAllAsync(ct);
    }
    public async Task<ListPage<ActionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        return await _repo.GetPagedAsync(request, ct);
    }
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(id, ct);
    }
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _repo.ExistsByNameAsync(name, ct);
    }

    public async Task<bool> CreateAsync(ActionEntity entity, CancellationToken ct)
    {
        return await _repo.AddAsync(entity, ct);
    }
    public async Task<bool> UpdateAsync(ActionEntity entity, CancellationToken ct)
    {
        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> ActivateAsync(ActionEntity entity, CancellationToken ct)
    {
        entity.Activate(entity.ModifiedBy);
        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeactivateAsync(ActionEntity entity, CancellationToken ct)
    {
        entity.Deactivate(entity.ModifiedBy);
        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeleteAsync(ActionEntity entity, CancellationToken ct)
    {
        entity.Delete(entity.ModifiedBy);
        return await _repo.UpdateAsync(entity, ct);
    }
}
