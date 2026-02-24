using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.Tenant;

public class UpdateTenantRouteValidator : AbstractValidator<UpdateTenantRequest>
{
    private readonly ILocalizationService _localization;

    public UpdateTenantRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Update.Name.MaximumLength", 200));
    }
}
