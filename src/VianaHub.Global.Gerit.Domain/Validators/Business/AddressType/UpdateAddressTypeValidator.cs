using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AddressType;

public class UpdateAddressTypeValidator : AbstractValidator<AddressTypeEntity>
{
    public UpdateAddressTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.IdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.AddressType.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.AddressType.NameMaxLength", 200));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.ModifiedByRequired"));
    }
}
