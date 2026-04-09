using FluentValidation;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AttachmentCategory;

public class CreateAttachmentCategoryRouteValidator : AbstractValidator<int>
{
    public CreateAttachmentCategoryRouteValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("attachment_category.id.invalid");
    }
}
