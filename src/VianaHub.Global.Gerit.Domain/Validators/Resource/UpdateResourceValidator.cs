using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Domain.Validators.Resource;

public class UpdateResourceValidator : AbstractValidator<ResourceEntity>
{
    public UpdateResourceValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Resource.InvalidId"));

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

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Resource.CannotUpdateDeleted"));
    }
}
