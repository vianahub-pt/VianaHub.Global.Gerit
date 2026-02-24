using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Equipment;

public class UpdateEquipmentValidator : AbstractValidator<EquipmentEntity>
{
    public UpdateEquipmentValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Equipment.InvalidId"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Equipment.TenantIdRequired"));

        RuleFor(x => x.EquipmentTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Equipment.EquipmentTypeIdRequired"));

        RuleFor(x => x.StatusId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Equipment.StatusIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Equipment.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.Equipment.NameMaxLength", 150));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Equipment.CannotUpdateDeleted"));
    }
}
