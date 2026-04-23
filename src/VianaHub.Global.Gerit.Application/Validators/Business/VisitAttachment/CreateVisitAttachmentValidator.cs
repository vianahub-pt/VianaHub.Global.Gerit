using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;

namespace VianaHub.Global.Gerit.Application.Validators.Business.VisitAttachment;

public class CreateVisitAttachmentValidator : AbstractValidator<CreateVisitAttachmentRequest>
{
    public CreateVisitAttachmentValidator()
    {
        RuleFor(x => x.FileTypeId)
            .GreaterThan(0)
            .WithMessage("visit_attachment.file_type_id.required");

        RuleFor(x => x.VisitId)
            .GreaterThan(0)
            .WithMessage("visit_attachment.visit_id.required");

        RuleFor(x => x.AttachmentCategoryId)
            .GreaterThan(0)
            .WithMessage("visit_attachment.attachment_category_id.required");

        RuleFor(x => x.S3Key)
            .NotEmpty()
            .WithMessage("visit_attachment.s3_key.required")
            .MaximumLength(500)
            .WithMessage("visit_attachment.s3_key.max_length");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("visit_attachment.file_name.required")
            .MaximumLength(255)
            .WithMessage("visit_attachment.file_name.max_length");

        RuleFor(x => x.FileSizeBytes)
            .GreaterThan(0)
            .WithMessage("visit_attachment.file_size.required");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("visit_attachment.display_order.invalid");
    }
}
