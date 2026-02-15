using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.Team;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.Team;

public class CreateTeamRouteValidator : AbstractValidator<CreateTeamRequest>
{
    public CreateTeamRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.Team.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.Team.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(localization.GetMessage("Api.Validator.Team.Create.Description.MaximumLength", 500));
    }
}
