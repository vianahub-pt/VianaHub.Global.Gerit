using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitContact;

/// <summary>
/// Validador de rota para criação de VisitContact
/// </summary>
public class CreateVisitContactRouteValidator : AbstractValidator<CreateVisitContactRequest>
{
    public CreateVisitContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.VisitId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.VisitTeamId"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContact.Create.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
