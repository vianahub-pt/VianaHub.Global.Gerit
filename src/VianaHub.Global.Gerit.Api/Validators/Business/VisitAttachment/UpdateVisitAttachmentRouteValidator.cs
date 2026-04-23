using FluentValidation;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitAttachment;

public class UpdateVisitAttachmentRouteValidator : AbstractValidator<int>
{
    public UpdateVisitAttachmentRouteValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("visit_attachment.id.invalid");
    }
}
