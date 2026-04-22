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
        //RuleFor(x => x.Name)
        //    .NotEmpty()
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Name"))
        //    .MaximumLength(150)
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Name.MaximumLength", 150));

        //RuleFor(x => x.Email)
        //    .NotEmpty()
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Email"))
        //    .MaximumLength(255)
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Email.MaximumLength", 255))
        //    .EmailAddress()
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Email.Invalid"));

        //RuleFor(x => x.Phone)
        //    .NotEmpty()
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Phone"))
        //    .MaximumLength(30)
        //    .WithMessage(localization.GetMessage("Api.Validator.Client.Create.Phone.MaximumLength", 30));
    }
}
