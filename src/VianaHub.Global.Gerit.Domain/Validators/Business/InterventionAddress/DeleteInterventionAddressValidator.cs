using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionAddress;

public class DeleteInterventionAddressValidator : AbstractValidator<InterventionAddressEntity>
{
    public DeleteInterventionAddressValidator(ILocalizationService localization)
    {
        // Placeholder for delete rules
    }
}
