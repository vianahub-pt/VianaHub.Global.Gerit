using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionContact;

/// <summary>
/// Validador de rota para atualização de InterventionContact
/// </summary>
public class UpdateInterventionContactRouteValidator : AbstractValidator<UpdateInterventionContactRequest>
{
    public UpdateInterventionContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Update.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Update.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Update.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Update.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
