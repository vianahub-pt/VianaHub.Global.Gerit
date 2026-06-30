using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ConsentOriginType;

public class CreateConsentOriginTypeValidator : AbstractValidator<ConsentOriginTypeEntity>
{
    public CreateConsentOriginTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Domain.ConsentOriginType.NameRequired"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Domain.ConsentOriginType.NameMaxLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage(localization.GetMessage("Domain.ConsentOriginType.DescriptionMaxLength", 300));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage(localization.GetMessage("Domain.ConsentOriginType.CreatedByRequired"));
    }
}
