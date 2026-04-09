using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitAddress;

public class DeactivateVisitAddressValidator : AbstractValidator<VisitAddressEntity>
{
    public DeactivateVisitAddressValidator(ILocalizationService localization)
    {
        // Placeholder for deactivation rules
    }
}
