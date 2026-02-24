using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.ReadModels;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Tools.Pagination;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

public class InterventionTeamEquipmentDomainService : IInterventionTeamEquipmentDomainService
{
    private readonly IInterventionTeamEquipmentDataRepository _repo;
    private readonly IEntityDomainValidator<InterventionTeamEquipmentEntity> _validator;
    private readonly INotify _notify;

    public InterventionTeamEquipmentDomainService(IInterventionTeamEquipmentDataRepository repo, IEntityDomainValidator<InterventionTeamEquipmentEntity> validator, INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<InterventionTeamEquipmentEntity> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _repo.GetByIdAsync(id, ct);
    }
    public async Task<IEnumerable<InterventionTeamEquipmentEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _repo.GetAllAsync(ct);
    }
    public async Task<ListPage<InterventionTeamEquipmentEntity>> GetPagedAsync(PagedFilter request, CancellationToken ct)
    {
        return await _repo.GetPagedAsync(request, ct);
    }
    public async Task<bool> ExistsByIdAsync(int tenantId, int interventionId, int equipmentId, CancellationToken ct)
    {
        return await _repo.ExistsByIdAsync(tenantId, interventionId, equipmentId, ct);
    }

    public async Task<bool> CreateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForCreateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        await _repo.AddAsync(entity, ct);
        return true;
    }
    public async Task<bool> UpdateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForUpdateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        await _repo.UpdateAsync(entity, ct);
        return true;
    }
    public async Task<bool> ActivateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForActivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        await _repo.UpdateAsync(entity, ct);
        return true;
    }
    public async Task<bool> DeactivateAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeactivateAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        await _repo.UpdateAsync(entity, ct);
        return true;
    }
    public async Task<bool> DeleteAsync(InterventionTeamEquipmentEntity entity, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateForDeleteAsync(entity);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                _notify.Add(error.ErrorMessage, 400);
            return false;
        }

        await _repo.UpdateAsync(entity, ct);
        return true;
    }
}
