using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AddressType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AddressType;

public class UpdateAddressTypeRouteValidator : AbstractValidator<UpdateAddressTypeRequest>
{
    public UpdateAddressTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Update.Name"))
            .MaximumLength(200).WithMessage(localization.GetMessage("Api.Validator.AddressType.Update.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Update.Description"))
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.AddressType.Update.Description.MaximumLength", 500));
    }
}
