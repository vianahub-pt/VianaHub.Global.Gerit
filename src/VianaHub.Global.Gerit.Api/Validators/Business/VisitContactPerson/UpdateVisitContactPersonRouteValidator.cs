using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.VisitContactPersons;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitContactPersons;

/// <summary>
/// Validador de rota para atualização de VisitContact
/// </summary>
public class UpdateVisitContactPersonRouteValidator : AbstractValidator<UpdateVisitContactPersonRequest>
{
    public UpdateVisitContactPersonRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.VisitContactPerson.Update.Name"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContactPerson.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.VisitContactPerson.Update.Email"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContactPerson.Update.Email.MaximumLength", 255))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Api.Validator.VisitContactPerson.Update.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Api.Validator.VisitContactPerson.Update.Phone.MaximumLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));
    }
}
