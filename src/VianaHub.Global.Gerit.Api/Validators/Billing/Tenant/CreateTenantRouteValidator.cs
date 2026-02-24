using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Billing.Tenant;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Billing.Tenant;

public class CreateTenantRouteValidator : AbstractValidator<CreateTenantRequest>
{
    private readonly ILocalizationService _localization;

    public CreateTenantRouteValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_localization.GetMessage("Api.Validator.Tenant.Create.Name"))
            .MaximumLength(200).WithMessage(_localization.GetMessage("Api.Validator.Tenant.Create.Name.MaximumLength", 200));
    }
}
