using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AddressType;

public class CreateAddressTypeValidator : AbstractValidator<AddressTypeEntity>
{
    public CreateAddressTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.CreatedByRequired"));
    }
}
