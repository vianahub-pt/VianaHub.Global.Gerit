using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionStatus;

/// <summary>
/// Validador para ativação de InterventionStatus
/// </summary>
public class ActivateInterventionStatusValidator : AbstractValidator<InterventionStatusEntity>
{
    public ActivateInterventionStatusValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.CannotActivateDeleted"));
    }
}
