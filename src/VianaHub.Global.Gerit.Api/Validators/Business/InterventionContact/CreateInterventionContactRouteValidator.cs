using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.InterventionContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionContact;

/// <summary>
/// Validador de rota para criação de InterventionContact
/// </summary>
public class CreateInterventionContactRouteValidator : AbstractValidator<CreateInterventionContactRequest>
{
    public CreateInterventionContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.InterventionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.InterventionTeamId"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.InterventionContact.Create.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
