using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientConsents;

public class UpdateClientConsentsRouteValidator : AbstractValidator<UpdateClientConsentsRequest>
{
    public UpdateClientConsentsRouteValidator()
    {
        RuleFor(x => x.RevokedDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("RevokedDate_FutureDate")
            .When(x => x.RevokedDate.HasValue);
    }
}
