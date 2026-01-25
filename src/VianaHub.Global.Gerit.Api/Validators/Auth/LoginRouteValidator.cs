using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;

namespace VianaHub.Global.Gerit.Api.Validators.Auth;

public class LoginRouteValidator : AbstractValidator<LoginRequest>
{
    public LoginRouteValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Application.Service.Auth.Login.InvalidTenantId");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Application.Service.Auth.Login.EmailRequired").EmailAddress().WithMessage("Application.Service.Auth.Login.EmailInvalid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Application.Service.Auth.Login.PasswordRequired");
    }
}
