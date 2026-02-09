using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AddressType;

public class ActivateAddressTypeValidator : AbstractValidator<AddressTypeEntity>
{
    public ActivateAddressTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.AddressType.CannotActivateDeleted"));
    }
}
