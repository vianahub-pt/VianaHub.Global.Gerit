using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Client;

/// <summary>
/// Validador para CreateClientRequest
/// </summary>
public class CreateClientRouteValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.ClientType)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.ClientType"));

        RuleFor(x => x.OriginType)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.OriginType"));

        RuleFor(x => x.UrlImage)
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.UrlImage.MaximumLength", 500))
            .When(x => !string.IsNullOrWhiteSpace(x.UrlImage));

        RuleFor(x => x.Note)
            .MaximumLength(1000)
            .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Note.MaximumLength", 1000))
            .When(x => !string.IsNullOrWhiteSpace(x.Note));
    }
}
