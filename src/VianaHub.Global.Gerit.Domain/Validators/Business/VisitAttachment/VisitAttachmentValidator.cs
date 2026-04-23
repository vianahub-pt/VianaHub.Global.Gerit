using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitAttachment;

public class VisitAttachmentValidator : IEntityDomainValidator<VisitAttachmentEntity>
{
    public async Task<ValidationResult> ValidateForCreateAsync(VisitAttachmentEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForUpdateAsync(VisitAttachmentEntity entity)
    {
        return await ValidateAsync(entity);
    }

    public async Task<ValidationResult> ValidateForActivateAsync(VisitAttachmentEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_attachment.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeactivateAsync(VisitAttachmentEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_attachment.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForDeleteAsync(VisitAttachmentEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_attachment.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    public async Task<ValidationResult> ValidateForRevokeAsync(VisitAttachmentEntity entity)
    {
        if (entity == null)
            return new ValidationResult(new[] { new ValidationFailure("Entity", "visit_attachment.entity.null") });

        return await Task.FromResult(new ValidationResult());
    }

    private async Task<ValidationResult> ValidateAsync(VisitAttachmentEntity entity)
    {
        var errors = new List<ValidationFailure>();

        if (entity == null)
        {
            errors.Add(new ValidationFailure("Entity", "visit_attachment.entity.null"));
            return new ValidationResult(errors);
        }

        if (entity.TenantId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.TenantId), "visit_attachment.tenant_id.invalid"));

        if (entity.FileTypeId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.FileTypeId), "visit_attachment.file_type_id.invalid"));

        if (entity.VisitId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.VisitId), "visit_attachment.visit_id.invalid"));

        if (entity.AttachmentCategoryId <= 0)
            errors.Add(new ValidationFailure(nameof(entity.AttachmentCategoryId), "visit_attachment.attachment_category_id.invalid"));

        if (string.IsNullOrWhiteSpace(entity.S3Key))
            errors.Add(new ValidationFailure(nameof(entity.S3Key), "visit_attachment.s3_key.required"));
        else if (entity.S3Key.Length > 500)
            errors.Add(new ValidationFailure(nameof(entity.S3Key), "visit_attachment.s3_key.max_length"));

        if (string.IsNullOrWhiteSpace(entity.FileName))
            errors.Add(new ValidationFailure(nameof(entity.FileName), "visit_attachment.file_name.required"));
        else if (entity.FileName.Length > 255)
            errors.Add(new ValidationFailure(nameof(entity.FileName), "visit_attachment.file_name.max_length"));

        if (entity.FileSizeBytes <= 0)
            errors.Add(new ValidationFailure(nameof(entity.FileSizeBytes), "visit_attachment.file_size.invalid"));

        if (entity.DisplayOrder < 0)
            errors.Add(new ValidationFailure(nameof(entity.DisplayOrder), "visit_attachment.display_order.invalid"));

        return await Task.FromResult(new ValidationResult(errors));
    }
}
