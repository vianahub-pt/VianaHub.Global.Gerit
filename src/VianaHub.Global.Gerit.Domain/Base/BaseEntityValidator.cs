using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Base;

/// <summary>
/// Classe base abstrata para implementação de validadores de entidades
/// </summary>
public abstract class BaseEntityValidator<TEntity> : IEntityDomainValidator<TEntity> where TEntity : class
{
    protected readonly ILocalizationService _localization;

    protected BaseEntityValidator(ILocalizationService localization)
    {
        _localization = localization;
    }

    public abstract Task<ValidationResult> ValidateForCreateAsync(TEntity entity);
    public abstract Task<ValidationResult> ValidateForUpdateAsync(TEntity entity);
    public abstract Task<ValidationResult> ValidateForActivateAsync(TEntity entity);
    public abstract Task<ValidationResult> ValidateForDeactivateAsync(TEntity entity);
    public abstract Task<ValidationResult> ValidateForDeleteAsync(TEntity entity);

    // Require explicit implementation for revoke validation
    public abstract Task<ValidationResult> ValidateForRevokeAsync(TEntity entity);
}
