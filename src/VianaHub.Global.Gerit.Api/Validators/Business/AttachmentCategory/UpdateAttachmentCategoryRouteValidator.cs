using FluentValidation;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AttachmentCategory;

public class UpdateAttachmentCategoryRouteValidator : AbstractValidator<int>
{
    public UpdateAttachmentCategoryRouteValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("attachment_category.id.invalid");
    }
}
