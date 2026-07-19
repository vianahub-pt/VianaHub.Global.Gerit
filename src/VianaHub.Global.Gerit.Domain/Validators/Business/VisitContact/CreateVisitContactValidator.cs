using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitContact;

/// <summary>
/// Validador para cria��o de VisitContact
/// </summary>
public class CreateVisitContactValidator : AbstractValidator<VisitContactPersonsEntity>
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

        RuleFor(x => x.JobTitle)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.VisitContact.JobTitleMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.JobTitle));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.VisitContact.DepartmentMaxLength", 100))
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.CellPhoneNumber)
            .MaximumLength(30)
            .WithMessage(localization.GetMessage("Domain.VisitContact.CellPhoneNumberMaxLength", 30))
            .When(x => !string.IsNullOrWhiteSpace(x.CellPhoneNumber));

        // Quando IsCellPhoneWhatsapp for true, CellPhoneNumber é obrigatório
        RuleFor(x => x.CellPhoneNumber)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.VisitContact.CellPhoneNumberRequiredForWhatsapp"))
            .When(x => x.IsCellPhoneWhatsapp);

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.VisitContact.CreatedByRequired"));
    }
}
