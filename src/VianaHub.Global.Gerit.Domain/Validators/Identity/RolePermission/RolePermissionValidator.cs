using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Identity;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Identity.RolePermission;

public class RolePermissionValidator : AbstractValidator<RolePermissionEntity>
{
    public RolePermissionValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.RolePermission.TenantIdRequired"));

        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.RolePermission.RoleIdRequired"));

        RuleFor(x => x.ResourceId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.RolePermission.ResourceIdRequired"));

        RuleFor(x => x.ActionId)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.RolePermission.ActionIdRequired"));
    }
}
