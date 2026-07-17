using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.TenantContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.TenantContact;

/// <summary>
/// Validador de rota para atualiza��o de TenantContact
/// </summary>
public class UpdateTenantContactRouteValidator : AbstractValidator<UpdateTenantContactRequest>
{
    public UpdateTenantContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Update.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Update.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Update.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.TenantContact.Update.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
