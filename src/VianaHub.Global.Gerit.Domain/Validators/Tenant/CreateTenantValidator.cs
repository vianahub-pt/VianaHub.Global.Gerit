using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Tenant;

public class CreateTenantValidator : AbstractValidator<TenantEntity>
{
    public CreateTenantValidator(ILocalizationService localization)
    {
        RuleFor(x => x.LegalName)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Tenant.LegalNameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Tenant.LegalNameMaxLength", 200));

        RuleFor(x => x.TradeName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.TradeName))
            .WithMessage(localization.GetMessage("Domain.Tenant.TradeNameMaxLength", 200));
    }
}
