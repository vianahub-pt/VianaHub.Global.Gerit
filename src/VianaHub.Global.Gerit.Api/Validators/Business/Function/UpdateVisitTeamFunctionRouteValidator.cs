using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Function;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Function;

public class UpdateVisitTeamFunctionRouteValidator : AbstractValidator<UpdateVisitTeamFunctionRequest>
{
    public UpdateVisitTeamFunctionRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Function.Update.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Function.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage(localization.GetMessage("Api.Validator.Function.Update.Description.MaximumLength", 255));
    }
}

