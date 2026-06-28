using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.AcquisitionSourceType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.AcquisitionSourceType;

public class CreateAcquisitionSourceTypeRouteValidator : AbstractValidator<CreateAcquisitionSourceTypeRequest>
{
    public CreateAcquisitionSourceTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Create.Code"))
            .MaximumLength(50).WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Create.Code.MaximumLength", 50));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Create.Name"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Create.Name.MaximumLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage(localization.GetMessage("Api.Validator.AcquisitionSourceType.Create.Description.MaximumLength", 300));
    }
}
