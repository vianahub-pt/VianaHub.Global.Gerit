using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Action;

public class CreateActionValidator : AbstractValidator<ActionEntity>
{
    public CreateActionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Action.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Action.NameMaxLength"));
    }
}
