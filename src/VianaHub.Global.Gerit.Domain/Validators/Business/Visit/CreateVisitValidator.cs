using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.Visit;

public class CreateVisitValidator : AbstractValidator<VisitEntity>
{
    public CreateVisitValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Visit.TenantIdRequired"));

        RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Visit.ClientIdRequired"));

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Visit.TitleRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.Visit.TitleMaxLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Visit.DescriptionRequired"))
            .MaximumLength(2000)
            .WithMessage(localization.GetMessage("Domain.Visit.DescriptionMaxLength", 2000));

        RuleFor(x => x.StartDateTime)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Visit.StartDateTimeRequired"));

        RuleFor(x => x.EstimatedValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localization.GetMessage("Domain.Visit.EstimatedValueGreaterThanOrEqual"));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Visit.CreatedByRequired"));
    }
}
