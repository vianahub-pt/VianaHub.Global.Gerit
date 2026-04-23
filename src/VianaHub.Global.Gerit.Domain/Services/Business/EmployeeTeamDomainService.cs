using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

public class EmployeeTeamDomainService : IEmployeeTeamDomainService
{
    private readonly IEmployeeTeamDataRepository _repo;
    private readonly IEntityDomainValidator<EmployeeTeamEntity> _validator;
    private readonly INotify _notify;

    public EmployeeTeamDomainService(IEmployeeTeamDataRepository repo, IEntityDomainValidator<EmployeeTeamEntity> validator, INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<EmployeeTeamEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.GetByIdAsync(id, ct);
    }
    public async Task<IEnumerable<EmployeeTeamEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _repo.GetAllAsync(ct);
    }
    public async Task<ListPage<EmployeeTeamEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        return await _repo.GetPagedAsync(request, ct);
    }
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(id, ct);
    }

    public async Task<bool> CreateAsync(EmployeeTeamEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForCreateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        return await _repo.AddAsync(entity, ct);
    }
    public async Task<bool> UpdateAsync(EmployeeTeamEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForUpdateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> ActivateAsync(EmployeeTeamEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForActivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeactivateAsync(EmployeeTeamEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeactivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
    public async Task<bool> DeleteAsync(EmployeeTeamEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeleteAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        return await _repo.UpdateAsync(entity, ct);
    }
}
