using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AddressType;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AddressType;

public class CreateAddressTypeRouteValidator : AbstractValidator<CreateAddressTypeRequest>
{
    public CreateAddressTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.TenantId"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Name"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Description"))
            .MaximumLength(255).WithMessage(localization.GetMessage("Api.Validator.AddressType.Create.Description.MaximumLength", 255));
    }
}
