using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Intervention;

public class UpdateInterventionValidator : AbstractValidator<InterventionEntity>
{
    public UpdateInterventionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.IdRequired"));

        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.ClientIdRequired"));

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Intervention.TitleRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Intervention.TitleMaxLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Intervention.DescriptionRequired"))
            .MaximumLength(2000)
            .WithMessage(localization.GetMessage("Domain.Intervention.DescriptionMaxLength", 2000));

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Intervention.StartDateTimeRequired"));

        RuleFor(x => x.EstimatedValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.EstimatedValueGreaterThanOrEqual"));

        When(x => x.RealValue.HasValue, () =>
        {
            RuleFor(x => x.RealValue.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage(localization.GetMessage("Domain.Intervention.RealValueGreaterThanOrEqual"));
        });

        When(x => x.EndDateTime.HasValue, () =>
        {
            RuleFor(x => x.EndDateTime.Value)
                .GreaterThan(x => x.StartDateTime)
                .WithMessage(localization.GetMessage("Domain.Intervention.EndDateTimeGreaterThanStart"));
        });

        RuleFor(x => x.IsDeleted)
            .Must(x => !x)
            .WithMessage(localization.GetMessage("Domain.Intervention.CannotUpdateDeleted"));
    }
}
