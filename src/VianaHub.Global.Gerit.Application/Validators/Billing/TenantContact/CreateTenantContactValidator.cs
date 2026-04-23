using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;

namespace VianaHub.Global.Gerit.Application.Validators.Billing.TenantContact;

public class CreateTenantContactValidator : AbstractValidator<CreateTenantContactRequest>
{
    public CreateTenantContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name_Required")
            .MaximumLength(200).WithMessage("Name_MaxLength");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email_Required")
            .MaximumLength(255).WithMessage("Email_MaxLength")
            .EmailAddress().WithMessage("Email_Invalid");

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Phone_MaxLength");
    }
}
