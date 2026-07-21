using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.PartyType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.PartyType;

/// <summary>
/// Validador de rota para criação de PartyType.
/// </summary>
public class CreatePartyTypeRouteValidator : AbstractValidator<CreatePartyTypeRequest>
{
    public CreatePartyTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.PartyType.Create.Code"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.PartyType.Create.Code.MaximumLength", 50));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.PartyType.Create.Name"))
            .MaximumLength(100)
            .WithMessage(localization.GetMessage("Api.Validator.PartyType.Create.Name.MaximumLength", 100));
    }
}
