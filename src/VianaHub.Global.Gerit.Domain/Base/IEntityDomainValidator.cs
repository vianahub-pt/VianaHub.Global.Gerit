using FluentValidation.Results;

namespace VianaHub.Global.Gerit.Domain.Base;

/// <summary>
/// Interface genérica para validação de entidades de domínio
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade a ser validada</typeparam>
public interface IEntityDomainValidator<TEntity> where TEntity : class
{
    /// <summary>
    /// Valida uma entidade para operação de criação
    /// </summary>
    Task<ValidationResult> ValidateForCreateAsync(TEntity entity);

    /// <summary>
    /// Valida uma entidade para operação de atualização
    /// </summary>
    Task<ValidationResult> ValidateForUpdateAsync(TEntity entity);

    /// <summary>
    /// Valida uma entidade para operação de ativação
    /// </summary>
    Task<ValidationResult> ValidateForActivateAsync(TEntity entity);

    /// <summary>
    /// Valida uma entidade para operação de desativação
    /// </summary>
    Task<ValidationResult> ValidateForDeactivateAsync(TEntity entity);

    /// <summary>
    /// Valida uma entidade para operação de exclusão
    /// </summary>
    Task<ValidationResult> ValidateForDeleteAsync(TEntity entity);

    /// <summary>
    /// Valida uma entidade para operação de revogação
    /// </summary>
    Task<ValidationResult> ValidateForRevokeAsync(TEntity entity);
}
