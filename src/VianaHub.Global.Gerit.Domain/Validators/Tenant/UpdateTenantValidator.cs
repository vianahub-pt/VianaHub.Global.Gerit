using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Tenant;

public class UpdateTenantValidator : AbstractValidator<TenantEntity>
{
    public UpdateTenantValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Tenant.InvalidId"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Tenant.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Tenant.NameMaxLength", 200));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Tenant.CannotUpdateDeleted"));
    }
}
