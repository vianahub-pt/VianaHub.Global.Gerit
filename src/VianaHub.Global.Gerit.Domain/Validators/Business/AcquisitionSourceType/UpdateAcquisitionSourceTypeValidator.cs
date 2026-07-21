using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AcquisitionSourceType;

public class UpdateAcquisitionSourceTypeValidator : AbstractValidator<AcquisitionSourceTypeEntity>
{
    public UpdateAcquisitionSourceTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.IdRequired"));

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.CodeRequired"))
            .MaximumLength(50).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.CodeMaxLength", 50));

        RuleFor(x => x.ModifiedBy)
            .NotNull().WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.ModifiedByRequired"))
            .GreaterThan(0).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.ModifiedByRequired"));
    }
}
