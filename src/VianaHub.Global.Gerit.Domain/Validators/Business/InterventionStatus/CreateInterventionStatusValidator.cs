using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionStatus;

/// <summary>
/// Validador para criação de InterventionStatus
/// </summary>
public class CreateInterventionStatusValidator : AbstractValidator<InterventionStatusEntity>
{
    public CreateInterventionStatusValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.NameMaxLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.DescriptionRequired"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.DescriptionMaxLength", 500));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.CreatedByRequired"));
    }
}
