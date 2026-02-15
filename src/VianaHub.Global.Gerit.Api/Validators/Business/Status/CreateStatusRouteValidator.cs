using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Status;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Status;

/// <summary>
/// Validador para CreateStatusRequest
/// </summary>
public class CreateStatusRouteValidator : AbstractValidator<CreateStatusRequest>
{
    public CreateStatusRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Status.Create.Name"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Api.Validator.Status.Create.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.Status.Create.Description"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Api.Validator.Status.Create.Description.MaximumLength", 500));
    }
}
