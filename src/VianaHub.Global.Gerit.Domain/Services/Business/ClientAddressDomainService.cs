using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

/// <summary>
/// Serviço de domínio para ClientAddress
/// </summary>
public class ClientAddressDomainService : IClientAddressDomainService
{
    private readonly IClientAddressDataRepository _repository;
    private readonly IEntityDomainValidator<ClientAddressEntity> _validator;
    private readonly INotify _notify;

    public ClientAddressDomainService(
        IClientAddressDataRepository repository,
        IEntityDomainValidator<ClientAddressEntity> validator,
        INotify notify)
    {
        _repository = repository;
        _validator = validator;
        _notify = notify;
    }

    public async Task<bool> CreateAsync(ClientAddressEntity entity, CancellationToken ct)
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

        return await _repository.AddAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(ClientAddressEntity entity, CancellationToken ct)
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

        return await _repository.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(ClientAddressEntity entity, CancellationToken ct)
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

        return await _repository.UpdateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(ClientAddressEntity entity, CancellationToken ct)
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

        return await _repository.UpdateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(ClientAddressEntity entity, CancellationToken ct)
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

        return await _repository.UpdateAsync(entity, ct);
    }
}
