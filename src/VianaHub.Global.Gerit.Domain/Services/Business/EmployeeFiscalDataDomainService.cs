using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Business;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Domain.Services.Business;

public class EmployeeFiscalDataDomainService : IEmployeeFiscalDataDomainService
{
    private readonly IEmployeeFiscalDataDataRepository _repository;
    private readonly IEntityDomainValidator<EmployeeFiscalDataEntity> _validator;
    private readonly INotify _notify;

    public EmployeeFiscalDataDomainService(
        IEmployeeFiscalDataDataRepository repository,
        IEntityDomainValidator<EmployeeFiscalDataEntity> validator,
        INotify notify)
    {
        _repository = repository;
        _validator = validator;
        _notify = notify;
    }

    public async Task<bool> CreateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default)
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

        return await _repository.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default)
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

    public async Task<bool> ActivateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default)
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

    public async Task<bool> DeactivateAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default)
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

    public async Task<bool> DeleteAsync(EmployeeFiscalDataEntity entity, CancellationToken ct = default)
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
