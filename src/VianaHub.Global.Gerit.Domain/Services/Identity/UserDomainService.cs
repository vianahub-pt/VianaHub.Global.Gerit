using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Interfaces.Domain;
using VianaHub.Global.Gerit.Domain.Interfaces.Repository;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;
using VianaHub.Global.Gerit.Domain.Validators.User;

namespace VianaHub.Global.Gerit.Domain.Services.Identity;

/// <summary>
/// Serviço de domínio para operações relacionadas a usuários
/// Centraliza validações e regras de negócio
/// </summary>
public class UserDomainService : IUserDomainService
{
    private readonly IUserDataRepository _repository;
    private readonly INotify _notify;
    private readonly ILocalizationService _localization;

    public UserDomainService(
        IUserDataRepository repository,
        INotify notify,
        ILocalizationService localization)
    {
        _repository = repository;
        _notify = notify;
        _localization = localization;
    }

    public async Task<bool> CreateAsync(UserEntity entity, CancellationToken ct)
    {
        // Valida a entidade
        var validator = new CreateUserValidator(_localization);
        var validationResult = await validator.ValidateAsync(entity, ct);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        // Persiste a entidade
        return await _repository.CreateAsync(entity, ct);
    }

    public async Task<bool> UpdateAsync(UserEntity entity, CancellationToken ct)
    {
        // Valida a entidade
        var validator = new UpdateUserValidator(_localization);
        var validationResult = await validator.ValidateAsync(entity, ct);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        // Persiste a entidade
        return await _repository.UpdateAsync(entity, ct);
    }

    public async Task<bool> ActivateAsync(UserEntity entity, CancellationToken ct)
    {
        // Valida a entidade
        var validator = new ActivateUserValidator(_localization);
        var validationResult = await validator.ValidateAsync(entity, ct);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        // Persiste a entidade
        return await _repository.UpdateAsync(entity, ct);
    }

    public async Task<bool> DeactivateAsync(UserEntity entity, CancellationToken ct)
    {
        // Valida a entidade
        var validator = new DeactivateUserValidator(_localization);
        var validationResult = await validator.ValidateAsync(entity, ct);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        // Persiste a entidade
        return await _repository.UpdateAsync(entity, ct);
    }

    public async Task<bool> DeleteAsync(UserEntity entity, CancellationToken ct)
    {
        // Valida a entidade
        var validator = new DeleteUserValidator(_localization);
        var validationResult = await validator.ValidateAsync(entity, ct);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _notify.Add(error.ErrorMessage, 400);
            }
            return false;
        }

        // Persiste a entidade
        return await _repository.UpdateAsync(entity, ct);
    }
}
