using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.Resource;

public class CreateResourceValidator : AbstractValidator<ResourceEntity>
{
    public CreateResourceValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Resource.NameRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.Resource.NameMaxLength", 100));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Resource.DescriptionRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.Resource.DescriptionMaxLength", 255));
    }
}
