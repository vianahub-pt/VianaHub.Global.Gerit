using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantFiscalData;

/// <summary>
/// Validador para desativação de TenantFiscalData
/// </summary>
public class DeactivateTenantFiscalDataValidator : AbstractValidator<TenantFiscalDataEntity>
{
    public DeactivateTenantFiscalDataValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.TenantFiscalData.CannotDeactivateDeleted"));
    }
}
