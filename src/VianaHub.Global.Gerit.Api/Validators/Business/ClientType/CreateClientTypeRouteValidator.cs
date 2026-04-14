using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Client;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientType;

public class CreateClientTypeRouteValidator : AbstractValidator<CreateClientTypeRequest>
{
    public CreateClientTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ClientType.Create.Name"))
            .MaximumLength(200).WithMessage(localization.GetMessage("Api.Validator.ClientType.Create.Name.MaximumLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.ClientType.Create.Description"))
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.ClientType.Create.Description.MaximumLength", 500));
    }
}
