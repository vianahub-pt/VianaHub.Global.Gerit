using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AcquisitionSourceType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AcquisitionSourceType;

public class UpdateAcquisitionSourceTypeRouteValidator : AbstractValidator<UpdateAcquisitionSourceTypeRequest>
{
    public UpdateAcquisitionSourceTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Update.Name"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Update.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Update.Description.MaximumLength", 300));
    }
}
