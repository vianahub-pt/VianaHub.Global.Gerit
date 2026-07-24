using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContactPerson;

/// <summary>
/// Validador para desativação de TenantContact
/// </summary>
public class DeactivateTenantContactPersonValidator : AbstractValidator<TenantContactPersonsEntity>
{
    public DeactivateTenantContactPersonValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.CannotDeactivateDeleted"));
    }
}
