using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Tenant;

public class CreateTenantValidator : AbstractValidator<TenantEntity>
{
    public CreateTenantValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Tenant.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Tenant.NameMaxLength", 200));
    }
}
