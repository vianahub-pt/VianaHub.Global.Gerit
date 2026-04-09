using FluentValidation;
using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.VisitTeamVehicles;

public class BulkUploadVisitTeamVehiclesRouteValidator : AbstractValidator<IFormFile>
{
    public BulkUploadVisitTeamVehiclesRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x).NotNull().WithMessage(localization.GetMessage("Api.Validator.VisitTeamVehicle.BulkUpload.FileRequired"));
    }
}
