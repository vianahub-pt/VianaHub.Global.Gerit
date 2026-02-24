using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AddressType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AddressType;

public class CreateAddressTypeRouteValidator : AbstractValidator<CreateAddressTypeRequest>
{
    public CreateAddressTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Name"))
            .MaximumLength(200).WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Description"))
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Description.MaximumLength", 500));
    }
}
