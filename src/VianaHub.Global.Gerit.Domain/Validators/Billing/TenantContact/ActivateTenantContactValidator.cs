using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContact;

/// <summary>
/// Validador para ativação de TenantContact
/// </summary>
public class ActivateTenantContactValidator : AbstractValidator<TenantContactEntity>
{
    public ActivateTenantContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContact.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.TenantContact.CannotActivateDeleted"));
    }
}
