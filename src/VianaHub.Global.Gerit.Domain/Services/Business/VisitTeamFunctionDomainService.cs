using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

public class VisitTeamFunctionDomainService : IVisitTeamFunctionDomainService
{
    private readonly IVisitTeamFunctionDataRepository _repo;
    private readonly IEntityDomainValidator<VisitTeamFunctionEntity> _validator;
    private readonly INotify _notify;

    public VisitTeamFunctionDomainService(
        IVisitTeamFunctionDataRepository repo,
        IEntityDomainValidator<VisitTeamFunctionEntity> validator,
        INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<VisitTeamFunctionEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.GetByIdAsync(id, ct);
    }
    public async Task<IEnumerable<VisitTeamFunctionEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _repo.GetAllAsync(ct);
    }
    public async Task<ListPage<VisitTeamFunctionEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        return await _repo.GetPagedAsync(request, ct);
    }
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(id, ct);
    }

    public async Task<bool> CreateAsync(VisitTeamFunctionEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForCreateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.AddAsync(entity, ct);
    }
    public async Task<bool> UpdateAsync(VisitTeamFunctionEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForUpdateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> ActivateAsync(VisitTeamFunctionEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForActivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeactivateAsync(VisitTeamFunctionEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeactivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeleteAsync(VisitTeamFunctionEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeleteAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
}

