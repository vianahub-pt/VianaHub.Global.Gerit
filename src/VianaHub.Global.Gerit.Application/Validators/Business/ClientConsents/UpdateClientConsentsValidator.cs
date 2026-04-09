using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientConsents;

public class UpdateClientConsentsValidator : AbstractValidator<UpdateClientConsentsRequest>
{
    public UpdateClientConsentsValidator()
    {
        RuleFor(x => x.RevokedDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("RevokedDate_FutureDate")
            .When(x => x.RevokedDate.HasValue);
    }
}
