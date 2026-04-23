using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitContact;

/// <summary>
/// Validador para criação de VisitContact
/// </summary>
public class CreateVisitContactValidator : AbstractValidator<VisitContactEntity>
{
    public CreateVisitContactValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.VisitContact.TenantIdRequired"));

        RuleFor(x => x.VisitId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.VisitContact.VisitIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.VisitContact.NameRequired"))
            .MaximumLength(150)
            .WithMessage(localization.GetMessage("Domain.VisitContact.NameMaxLength"));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.VisitContact.EmailRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.VisitContact.EmailMaxLength"))
            .EmailAddress()
            .WithMessage(localization.GetMessage("Domain.VisitContact.EmailInvalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.VisitContact.PhoneMaxLength"))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.VisitContact.CreatedByRequired"));
    }
}
