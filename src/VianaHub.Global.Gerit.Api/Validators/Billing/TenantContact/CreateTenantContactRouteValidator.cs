using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.TenantContact;

/// <summary>
/// Validador de rota para cria��o de TenantContact
/// </summary>
public class CreateTenantContactRouteValidator : AbstractValidator<CreateTenantContactRequest>
{
    public CreateTenantContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Create.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Create.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Create.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Create.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
