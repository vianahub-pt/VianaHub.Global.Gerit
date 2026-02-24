using FluentValidation;
using Microsoft.AspNetCore.Http;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.InterventionTeamVehicles;

public class BulkUploadInterventionTeamVehiclesRouteValidator : AbstractValidator<IFormFile>
{
    public BulkUploadInterventionTeamVehiclesRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x).NotNull().WithMessage(localization.GetMessage("Api.Validator.InterventionTeamVehicle.BulkUpload.FileRequired"));
    }
}
