using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Intervention;

public class DeactivateInterventionValidator : AbstractValidator<InterventionEntity>
{
    public DeactivateInterventionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Must(x => !x)
            .WithMessage(localization.GetMessage("Domain.Intervention.CannotDeactivateDeleted"));
    }
}
