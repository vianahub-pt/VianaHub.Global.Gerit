using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMemberContact;

public class UpdateTeamMemberContactRouteValidator : AbstractValidator<UpdateTeamMemberContactRequest>
{
    public UpdateTeamMemberContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Update.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Update.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Update.Email"))
            .MaximumLength(255).WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Update.Email.MaximumLength", 255))
            .EmailAddress().WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Update.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Update.Phone.MaximumLength", 30));
    }
}
