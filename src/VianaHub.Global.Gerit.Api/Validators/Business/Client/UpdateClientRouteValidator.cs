using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Client;

/// <summary>
/// Validador para UpdateClientRequest
/// </summary>
public class UpdateClientRouteValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientType)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.ClientType"));

        RuleFor(x => x.OriginType)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Client.Update.OriginType"));
    }
}
