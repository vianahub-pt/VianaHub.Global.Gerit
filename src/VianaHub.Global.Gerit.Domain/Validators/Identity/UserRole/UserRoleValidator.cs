using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.UserRole;

public class UserRoleValidator : AbstractValidator<UserRoleEntity>
{
    public UserRoleValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Role.TenantIdRequired"));

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.User.InvalidId"));

        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.Role.InvalidId"));
    }
}
