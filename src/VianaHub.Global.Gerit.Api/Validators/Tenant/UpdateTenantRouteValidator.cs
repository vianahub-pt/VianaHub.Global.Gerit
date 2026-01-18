using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Tenant;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Tenant;

public class UpdateTenantRouteValidator : AbstractValidator<UpdateTenantRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateTenantRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.LegalName)
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Update.LegalName.MaximumLength", 200));

        RuleFor(x => x.TradeName)
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Update.TradeName.MaximumLength", 200));
    }
}
