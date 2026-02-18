using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.TeamMembersTeams;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.TeamMembersTeams;

public class BulkUploadTeamMembersTeamsRouteValidator : AbstractValidator<IFormFile>
{
    public BulkUploadTeamMembersTeamsRouteValidator(ILocalizationService localization)
    {
        // Basic validation left to FileValidationFilter; this ensures file is present
        RuleFor(x => x).NotNull().WithMessage(localization.GetMessage("Api.Validator.TeamMembersTeam.BulkUpload.FileRequired"));
    }
}
