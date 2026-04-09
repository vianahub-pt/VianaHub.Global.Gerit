using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientConsents;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientConsents;

public class CreateClientConsentsValidator : AbstractValidator<CreateClientConsentsRequest>
{
    public CreateClientConsentsValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientId_Required");

        RuleFor(x => x.ConsentTypeId)
            .GreaterThan(0).WithMessage("ConsentTypeId_Required");

        RuleFor(x => x.GrantedDate)
            .NotEmpty().WithMessage("GrantedDate_Required");

        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin_Required")
            .MaximumLength(50).WithMessage("Origin_MaxLength")
            .Must(origin => new[] { "Web", "Mobile", "Paper", "API" }.Contains(origin))
            .WithMessage("Origin_Invalid");

        RuleFor(x => x.IpAddress)
            .MaximumLength(50).WithMessage("IpAddress_MaxLength");

        RuleFor(x => x.UserAgent)
            .MaximumLength(500).WithMessage("UserAgent_MaxLength");
    }
}
