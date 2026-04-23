using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitAddress;

public class DeleteVisitAddressValidator : AbstractValidator<VisitAddressEntity>
{
    public DeleteVisitAddressValidator(ILocalizationService localization)
    {
        // Placeholder for delete rules
    }
}
