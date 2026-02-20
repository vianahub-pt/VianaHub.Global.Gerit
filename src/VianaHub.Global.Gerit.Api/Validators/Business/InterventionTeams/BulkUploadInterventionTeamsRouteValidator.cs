using FluentValidation;
using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Domain.Interfaces;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionTeams;

public class BulkUploadInterventionTeamsRouteValidator : AbstractValidator<IFormFile>
{
    public BulkUploadInterventionTeamsRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x).NotNull().WithMessage(localization.GetMessage("Api.Validator.InterventionTeam.BulkUpload.FileRequired"));
    }
}
