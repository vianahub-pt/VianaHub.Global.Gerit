using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.EmployeeTeams;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.EmployeeTeams;

public class BulkUploadEmployeeTeamRouteValidator : AbstractValidator<IFormFile>
{
    public BulkUploadEmployeeTeamRouteValidator(ILocalizationService localization)
    {
        // Basic validation left to FileValidationFilter; this ensures file is present
        RuleFor(x => x).NotNull().WithMessage(localization.GetMessage("Api.Validator.EmployeeTeam.BulkUpload.FileRequired"));
    }
}
