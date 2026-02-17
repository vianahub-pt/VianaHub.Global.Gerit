using System.Threading;
using System.Threading.Tasks;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

/// <summary>
/// Serviço de domínio para InterventionAddress
/// </summary>
public class InterventionAddressDomainService : IInterventionAddressDomainService
{
    private readonly IInterventionAddressDataRepository _repo;
    private readonly IEntityDomainValidator<InterventionAddressEntity> _validator;
    private readonly INotify _notify;

    public InterventionAddressDomainService(
        IInterventionAddressDataRepository repo,
        IEntityDomainValidator<InterventionAddressEntity> validator,
        INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<bool> CreateAsync(InterventionAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> UpdateAsync(InterventionAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> ActivateAsync(InterventionAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> DeactivateAsync(InterventionAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> DeleteAsync(InterventionAddressEntity entity, CancellationToken ct)
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
