using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AddressType;

public class CreateAddressTypeValidator : AbstractValidator<AddressTypeEntity>
{
    public CreateAddressTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.AddressType.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.AddressType.NameMaxLength", 200));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.CreatedByRequired"));
    }
}
