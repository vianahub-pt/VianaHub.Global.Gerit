using FluentValidation;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitAttachment;

public class CreateVisitAttachmentRouteValidator : AbstractValidator<int>
{
    public CreateVisitAttachmentRouteValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("visit_attachment.id.invalid");
    }
}
