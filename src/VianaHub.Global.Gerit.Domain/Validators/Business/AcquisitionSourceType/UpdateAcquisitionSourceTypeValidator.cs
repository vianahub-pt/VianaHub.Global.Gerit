using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.AcquisitionSourceType;

public class UpdateAcquisitionSourceTypeValidator : AbstractValidator<AcquisitionSourceTypeEntity>
{
    public UpdateAcquisitionSourceTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.IdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.NameRequired"))
            .MaximumLength(100).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.NameMaxLength", 100));

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.DescriptionMaxLength", 300));

        RuleFor(x => x.ModifiedBy)
            .NotNull().WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.ModifiedByRequired"))
            .GreaterThan(0).WithMessage(localization.GetMessage("Domain.AcquisitionSourceType.ModifiedByRequired"));
    }
}
