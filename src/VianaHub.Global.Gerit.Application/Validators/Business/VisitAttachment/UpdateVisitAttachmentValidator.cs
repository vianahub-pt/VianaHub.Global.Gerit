using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitAttachment;

namespace VianaHub.Global.Gerit.Application.Validators.Business.VisitAttachment;

public class UpdateVisitAttachmentValidator : AbstractValidator<UpdateVisitAttachmentRequest>
{
    public UpdateVisitAttachmentValidator()
    {
        RuleFor(x => x.AttachmentCategoryId)
            .GreaterThan(0)
            .WithMessage("visit_attachment.attachment_category_id.required");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("visit_attachment.file_name.required")
            .MaximumLength(255)
            .WithMessage("visit_attachment.file_name.max_length");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("visit_attachment.display_order.invalid");
    }
}
