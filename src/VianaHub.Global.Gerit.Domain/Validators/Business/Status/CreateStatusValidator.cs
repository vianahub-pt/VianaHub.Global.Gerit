using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Status;

/// <summary>
/// Validador para criação de Status
/// </summary>
public class CreateStatusValidator : AbstractValidator<StatusEntity>
{
    public CreateStatusValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Status.TenantIdRequired"));

        RuleFor(x => x.StatusTypeId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Status.StatusTypeIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Status.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Status.NameMaxLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Status.DescriptionRequired"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.Status.DescriptionMaxLength", 500));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Status.CreatedByRequired"));
    }
}
