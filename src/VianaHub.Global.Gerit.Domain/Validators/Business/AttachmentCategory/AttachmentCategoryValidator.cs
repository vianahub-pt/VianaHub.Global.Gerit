using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AttachmentCategory;

public class AttachmentCategoryValidator : IEntityDomainValidator<AttachmentCategoryEntity>
{
    public async Task<ValidationResult> ValidateForCreateAsync(AttachmentCategoryEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForUpdateAsync(AttachmentCategoryEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForActivateAsync(AttachmentCategoryEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "attachment_category.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.IsSystem)
            errors.Add(new ValidationFailure(nameof(entity.IsSystem), "attachment_category.system_category_cannot_be_modified"));

        return await Task.FromResult(new ValidationResult(errors));
    }

    public async Task<ValidationResult> ValidateForDeactivateAsync(AttachmentCategoryEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "attachment_category.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.IsSystem)
            errors.Add(new ValidationFailure(nameof(entity.IsSystem), "attachment_category.system_category_cannot_be_deactivated"));

        return await Task.FromResult(new ValidationResult(errors));
    }

    public async Task<ValidationResult> ValidateForDeleteAsync(AttachmentCategoryEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "attachment_category.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.IsSystem)
            errors.Add(new ValidationFailure(nameof(entity.IsSystem), "attachment_category.system_category_cannot_be_deleted"));

        return await Task.FromResult(new ValidationResult(errors));
    }

    public async Task<ValidationResult> ValidateForRevokeAsync(AttachmentCategoryEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "attachment_category.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    private async Task<ValidationResult> ValidateAsync(AttachmentCategoryEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "attachment_category.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.TenantId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.TenantId), "attachment_category.tenant_id.invalid"));

        if (string.IsNullOrWhiteSpace(entity.Name))
            errors.Add(new ValidationFailure(nameof(entity.Name), "attachment_category.name.required"));
        else if (entity.Name.Length > 100)
            errors.Add(new ValidationFailure(nameof(entity.Name), "attachment_category.name.max_length"));

        if (string.IsNullOrWhiteSpace(entity.Description))
            errors.Add(new ValidationFailure(nameof(entity.Description), "attachment_category.description.required"));
        else if (entity.Description.Length > 300)
            errors.Add(new ValidationFailure(nameof(entity.Description), "attachment_category.description.max_length"));

        if (entity.DisplayOrder < 0)
            errors.Add(new ValidationFailure(nameof(entity.DisplayOrder), "attachment_category.display_order.invalid"));

        return await Task.FromResult(new ValidationResult(errors));
    }
}
