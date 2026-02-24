using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.Role;

public class UpdateRoleValidator : AbstractValidator<RoleEntity>
{
    public UpdateRoleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Role.InvalidId"));

        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Role.TenantIdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Role.NameRequired"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Domain.Role.NameMaxLength", 100));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.Role.DescriptionRequired"))
            .MaximumLength(255)
            .WithMessage(localization.GetMessage("Domain.Role.DescriptionMaxLength", 255));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(localization.GetMessage("Domain.Role.CannotUpdateDeleted"));
    }
}
