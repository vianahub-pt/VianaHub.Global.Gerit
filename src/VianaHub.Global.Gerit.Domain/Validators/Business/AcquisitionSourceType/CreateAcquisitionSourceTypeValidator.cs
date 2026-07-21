using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AcquisitionSourceType;

public class CreateAcquisitionSourceTypeValidator : AbstractValidator<AcquisitionSourceTypeEntity>
{
    public CreateAcquisitionSourceTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.CodeRequired"))
            .MaximumLength(50).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.CodeMaxLength", 50));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.CreatedByRequired"));
    }
}
