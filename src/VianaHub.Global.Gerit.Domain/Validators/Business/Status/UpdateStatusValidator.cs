using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Status;

/// <summary>
/// Validador para atualização de Status
/// </summary>
public class UpdateStatusValidator : AbstractValidator<StatusEntity>
{
    public UpdateStatusValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Status.IdRequired"));

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

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Status.CannotUpdateDeleted"));
    }
}
