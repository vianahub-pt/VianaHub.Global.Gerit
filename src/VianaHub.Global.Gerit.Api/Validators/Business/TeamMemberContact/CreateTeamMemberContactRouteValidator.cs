using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMemberContact;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMemberContact;

public class CreateTeamMemberContactRouteValidator : AbstractValidator<CreateTeamMemberContactRequest>
{
    public CreateTeamMemberContactRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.TeamMemberId)
            .GreaterThan(0).WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.TeamMemberId"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.Name"))
            .MaximumLength(150).WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.Name.MaximumLength", 150));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.Email"))
            .MaximumLength(255).WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.Email.MaximumLength", 255))
            .EmailAddress().WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.Email.Invalid"));

        RuleFor(x => x.Phone)
            .MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage(localization.GetMessage("Api.Validator.TeamMemberContact.Create.Phone.MaximumLength", 30));
    }
}
