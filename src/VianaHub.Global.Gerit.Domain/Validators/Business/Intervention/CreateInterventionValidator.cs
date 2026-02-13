using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Intervention;

public class CreateInterventionValidator : AbstractValidator<InterventionEntity>
{
    public CreateInterventionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.TenantIdRequired"));

        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.ClientIdRequired"));

        RuleFor(x => x.TeamMemberId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.TeamMemberIdRequired"));

        RuleFor(x => x.VehicleId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.VehicleIdRequired"));

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

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Intervention.CreatedByRequired"));
    }
}
