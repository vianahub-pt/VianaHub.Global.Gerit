using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionAddress;

public class DeactivateInterventionAddressValidator : AbstractValidator<InterventionAddressEntity>
{
    public DeactivateInterventionAddressValidator(ILocalizationService localization)
    {
        // Placeholder for deactivation rules
    }
}
