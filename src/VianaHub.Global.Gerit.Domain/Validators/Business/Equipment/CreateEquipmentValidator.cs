using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Equipment;

public class CreateEquipmentValidator : AbstractValidator<EquipmentEntity>
{
    public CreateEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Equipment.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Equipment.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Equipment.NameMaxLength", 150));
    }
}
