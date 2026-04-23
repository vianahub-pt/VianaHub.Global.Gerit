using FluentValidation;
using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitTeams;

public class BulkUploadVisitTeamsRouteValidator : AbstractValidator<IFormFile>
{
    public BulkUploadVisitTeamsRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x).NotNull().WithMessage(localization.GetMessage("Api.Validator.VisitTeam.BulkUpload.FileRequired"));
    }
}
