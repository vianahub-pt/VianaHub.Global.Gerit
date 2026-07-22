using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitAttachment;

public class VisitAttachmentValidator : IEntityDomainValidator<VisitAttachmentsEntity>
{
    public async Task<ValidationResult> ValidateForCreateAsync(VisitAttachmentsEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForUpdateAsync(VisitAttachmentsEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForActivateAsync(VisitAttachmentsEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitAttachment.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeactivateAsync(VisitAttachmentsEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitAttachment.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeleteAsync(VisitAttachmentsEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitAttachment.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForRevokeAsync(VisitAttachmentsEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "Domain.VisitAttachment.EntityNull") });

        return await Task.FromResult(new ValidationResult());
    }

    private async Task<ValidationResult> ValidateAsync(VisitAttachmentsEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "Domain.VisitAttachment.EntityNull"));
            return new ValidationResult(errors);
        }

        if (entity.TenantId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.TenantId), "Domain.VisitAttachment.TenantIdInvalid"));

        if (entity.FileTypeId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.FileTypeId), "Domain.VisitAttachment.FileTypeIdInvalid"));

        if (entity.VisitId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.VisitId), "Domain.VisitAttachment.VisitIdInvalid"));

        if (string.IsNullOrWhiteSpace(entity.S3Key))
            errors.Add(new ValidationFailure(nameof(entity.S3Key), "Domain.VisitAttachment.S3KeyRequired"));
        else if (entity.S3Key.Length > 500)
            errors.Add(new ValidationFailure(nameof(entity.S3Key), "Domain.VisitAttachment.S3KeyMaxLength"));

        if (string.IsNullOrWhiteSpace(entity.FileName))
            errors.Add(new ValidationFailure(nameof(entity.FileName), "Domain.VisitAttachment.FileNameRequired"));
        else if (entity.FileName.Length > 255)
            errors.Add(new ValidationFailure(nameof(entity.FileName), "Domain.VisitAttachment.FileNameMaxLength"));

        if (entity.FileSizeBytes <= 0)
            errors.Add(new ValidationFailure(nameof(entity.FileSizeBytes), "Domain.VisitAttachment.FileSizeInvalid"));

        if (entity.DisplayOrder < 0)
            errors.Add(new ValidationFailure(nameof(entity.DisplayOrder), "Domain.VisitAttachment.DisplayOrderInvalid"));

        return await Task.FromResult(new ValidationResult(errors));
    }
}
