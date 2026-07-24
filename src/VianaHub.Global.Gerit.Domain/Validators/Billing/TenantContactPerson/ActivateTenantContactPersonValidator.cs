using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Billing;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Billing.TenantContactPerson;

/// <summary>
/// Validador para ativação de TenantContact
/// </summary>
public class ActivateTenantContactPersonValidator : AbstractValidator<TenantContactPersonsEntity>
{
    public ActivateTenantContactPersonValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.IdRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.TenantContactPerson.CannotActivateDeleted"));
    }
}
