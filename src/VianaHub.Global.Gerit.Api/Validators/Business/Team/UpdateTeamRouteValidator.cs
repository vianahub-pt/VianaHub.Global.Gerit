using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Team;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Team;

public class UpdateTeamRouteValidator : AbstractValidator<UpdateTeamRequest>
{
    public UpdateTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Team.Update.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Team.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.Team.Update.Description.MaximumLength", 500));
    }
}
