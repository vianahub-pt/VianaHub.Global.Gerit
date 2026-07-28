using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContactPerson;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.TenantContactPerson;

/// <summary>
/// Validador de rota para cria��o de TenantContact
/// </summary>
public class CreateTenantContactPersonRouteValidator : AbstractValidator<CreateTenantContactPersonRequest>
{
    public CreateTenantContactPersonRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContactPerson.Create.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContactPerson.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContactPerson.Create.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContactPerson.Create.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContactPerson.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContactPerson.Create.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
