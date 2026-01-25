using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;

namespace VianaHub.Global.Gerit.Api.Validators.Auth;

public class RefreshRouteValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRouteValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Application.Service.Auth.Refresh.InvalidTenantId");
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Application.Service.Auth.Refresh.TokenRequired");
    }
}
