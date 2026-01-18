using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Tenant;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Tenant;

public class CreateTenantRouteValidator : AbstractValidator<CreateTenantRequest>
{
    private readonly ILocalizationService _localization;

    public CreateTenantRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Tenant.Create.LegalName"))
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Create.LegalName.MaximumLength", 200));

        RuleFor(x => x.TradeName)
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Create.TradeName.MaximumLength", 200));
    }
}
