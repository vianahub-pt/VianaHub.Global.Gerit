using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AddressType;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AddressType;

public class UpdateAddressTypeRequestValidator : AbstractValidator<UpdateAddressTypeRequest>
{
    public UpdateAddressTypeRequestValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Update.Name"))
            .MaximumLength(200).WithMessage(localization.GetMessage("Api.Validator.AddressType.Update.Name.MaximumLength", 200));
    }
}
