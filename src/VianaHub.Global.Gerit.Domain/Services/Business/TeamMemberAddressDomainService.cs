using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

/// <summary>
/// Serviço de domínio para TeamMemberAddress
/// </summary>
public class TeamMemberAddressDomainService : ITeamMemberAddressDomainService
{
    private readonly ITeamMemberAddressDataRepository _repo;
    private readonly IEntityDomainValidator<TeamMemberAddressEntity> _validator;
    private readonly INotify _notify;

    public TeamMemberAddressDomainService(
        ITeamMemberAddressDataRepository repo,
        IEntityDomainValidator<TeamMemberAddressEntity> validator,
        INotify notify)
    {
        _repo = repo;
        _validator = validator;
        _notify = notify;
    }

    public async Task<bool> CreateAsync(TeamMemberAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> UpdateAsync(TeamMemberAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> ActivateAsync(TeamMemberAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> DeactivateAsync(TeamMemberAddressEntity entity, CancellationToken ct)
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

    public async Task<bool> DeleteAsync(TeamMemberAddressEntity entity, CancellationToken ct)
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
