using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientAddress;

/// <summary>
/// Validador para desativação de ClientAddress
/// </summary>
public class DeactivateClientAddressValidator : AbstractValidator<ClientAddressEntity>
{
    public DeactivateClientAddressValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.ClientAddress.CannotDeactivateDeleted"));
    }
}
