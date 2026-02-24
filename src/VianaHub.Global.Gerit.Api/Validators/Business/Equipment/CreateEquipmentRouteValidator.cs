using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Equipment;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Equipment;

public class CreateEquipmentRouteValidator : AbstractValidator<CreateEquipmentRequest>
{
    public CreateEquipmentRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Equipment.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Equipment.Create.Name.MaximumLength", 150));

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.Equipment.Create.SerialNumber.MaximumLength", 100));
    }
}
