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

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Update.Name.MaximumLength", 200));
    }
}
