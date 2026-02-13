using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionStatus;

/// <summary>
/// Validador para atualização de InterventionStatus
/// </summary>
public class UpdateInterventionStatusValidator : AbstractValidator<InterventionStatusEntity>
{
    public UpdateInterventionStatusValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.IdRequired"));

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

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.InterventionStatus.CannotUpdateDeleted"));
    }
}
