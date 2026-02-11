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

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.AddressType.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.AddressType.ModifiedByRequired"));
    }
}
