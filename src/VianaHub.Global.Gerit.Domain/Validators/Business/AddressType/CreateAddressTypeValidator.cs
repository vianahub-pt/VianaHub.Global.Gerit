using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AddressType;

public class CreateAddressTypeValidator : AbstractValidator<AddressTypeEntity>
{
    public CreateAddressTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.AddressType.NameRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.AddressType.NameMaxLength", 100));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.AddressType.DescriptionRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.AddressType.DescriptionMaxLength", 255));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.CreatedByRequired"));
    }
}
