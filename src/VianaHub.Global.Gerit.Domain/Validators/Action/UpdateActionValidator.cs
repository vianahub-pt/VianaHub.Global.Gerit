using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Action;

public class UpdateActionValidator : AbstractValidator<ActionEntity>
{
    public UpdateActionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Action.IdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Action.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Action.NameMaxLength"));
    }
}
