using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AttachmentCategory;

namespace VianaHub.Global.Gerit.Application.Validators.Business.AttachmentCategory;

public class CreateAttachmentCategoryValidator : AbstractValidator<CreateAttachmentCategoryRequest>
{
    public CreateAttachmentCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("attachment_category.name.required")
            .MaximumLength(100)
            .WithMessage("attachment_category.name.max_length");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("attachment_category.description.required")
            .MaximumLength(300)
            .WithMessage("attachment_category.description.max_length");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("attachment_category.display_order.invalid");
    }
}
