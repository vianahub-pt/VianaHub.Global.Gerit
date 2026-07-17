using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantAddress;

/// <summary>
/// Validador para ativação de TenantAddress
/// </summary>
public class ActivateTenantAddressValidator : AbstractValidator<TenantAddressesEntity>
{
    public ActivateTenantAddressValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.TenantAddress.CannotActivateDeleted"));
    }
}
