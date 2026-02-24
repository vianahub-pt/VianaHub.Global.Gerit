using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionContact;

/// <summary>
/// Validador para atualização de InterventionContact
/// </summary>
public class UpdateInterventionContactValidator : AbstractValidator<InterventionContactEntity>
{
    public UpdateInterventionContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.IdRequired"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.TenantIdRequired"));

        RuleFor(x => x.InterventionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.InterventionIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.InterventionContact.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.NameMaxLength"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.InterventionContact.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.EmailMaxLength"))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.InterventionContact.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.PhoneMaxLength"))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.ModifiedByRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.InterventionContact.IsDeleted"));
    }
}
