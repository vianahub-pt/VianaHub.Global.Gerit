using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Auth;

namespace VianaHub.Global.Gerit.Api.Validators.Auth;

public class RegisterRouteValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRouteValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Application.Service.Auth.Register.InvalidTenantId");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Application.Service.Auth.Register.NameRequired");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Application.Service.Auth.Register.EmailRequired").EmailAddress().WithMessage("Application.Service.Auth.Register.EmailInvalid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Application.Service.Auth.Register.PasswordRequired").MinimumLength(8).WithMessage("Application.Service.Auth.Register.PasswordTooShort");
    }
}
