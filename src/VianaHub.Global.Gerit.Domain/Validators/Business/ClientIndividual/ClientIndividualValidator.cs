using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientIndividual;

public class ClientIndividualValidator : IEntityDomainValidator<ClientIndividualEntity>
{
    public async Task<ValidationResult> ValidateForCreateAsync(ClientIndividualEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForUpdateAsync(ClientIndividualEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForActivateAsync(ClientIndividualEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "client_individual.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.IsDeleted)
            errors.Add(new ValidationFailure(nameof(entity.IsDeleted), "client_individual.cannot_activate_deleted"));

        return await Task.FromResult(new ValidationResult(errors));
    }

    public async Task<ValidationResult> ValidateForDeactivateAsync(ClientIndividualEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "client_individual.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.IsDeleted)
            errors.Add(new ValidationFailure(nameof(entity.IsDeleted), "client_individual.cannot_deactivate_deleted"));

        return await Task.FromResult(new ValidationResult(errors));
    }

    public async Task<ValidationResult> ValidateForDeleteAsync(ClientIndividualEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "client_individual.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.IsDeleted)
            errors.Add(new ValidationFailure(nameof(entity.IsDeleted), "client_individual.already_deleted"));

        return await Task.FromResult(new ValidationResult(errors));
    }

    public async Task<ValidationResult> ValidateForRevokeAsync(ClientIndividualEntity entity)
    {
        return await ValidateForDeleteAsync(entity);
    }

    private async Task<ValidationResult> ValidateAsync(ClientIndividualEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "client_individual.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.TenantId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.TenantId), "client_individual.tenant_id.invalid"));

        if (entity.ClientId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.ClientId), "client_individual.client_id.invalid"));

        if (string.IsNullOrWhiteSpace(entity.FirstName))
            errors.Add(new ValidationFailure(nameof(entity.FirstName), "client_individual.first_name.required"));
        else if (entity.FirstName.Length > 100)
            errors.Add(new ValidationFailure(nameof(entity.FirstName), "client_individual.first_name.max_length"));

        if (string.IsNullOrWhiteSpace(entity.LastName))
            errors.Add(new ValidationFailure(nameof(entity.LastName), "client_individual.last_name.required"));
        else if (entity.LastName.Length > 100)
            errors.Add(new ValidationFailure(nameof(entity.LastName), "client_individual.last_name.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.PhoneNumber) && entity.PhoneNumber.Length > 50)
            errors.Add(new ValidationFailure(nameof(entity.PhoneNumber), "client_individual.phone_number.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.CellPhoneNumber) && entity.CellPhoneNumber.Length > 50)
            errors.Add(new ValidationFailure(nameof(entity.CellPhoneNumber), "client_individual.cell_phone_number.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.Email) && entity.Email.Length > 500)
            errors.Add(new ValidationFailure(nameof(entity.Email), "client_individual.email.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.Gender) && entity.Gender.Length > 20)
            errors.Add(new ValidationFailure(nameof(entity.Gender), "client_individual.gender.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.DocumentType) && entity.DocumentType.Length > 50)
            errors.Add(new ValidationFailure(nameof(entity.DocumentType), "client_individual.document_type.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.DocumentNumber) && entity.DocumentNumber.Length > 50)
            errors.Add(new ValidationFailure(nameof(entity.DocumentNumber), "client_individual.document_number.max_length"));

        if (!string.IsNullOrWhiteSpace(entity.Nationality) && entity.Nationality.Length != 2)
            errors.Add(new ValidationFailure(nameof(entity.Nationality), "client_individual.nationality.invalid_length"));

        return await Task.FromResult(new ValidationResult(errors));
    }
}
