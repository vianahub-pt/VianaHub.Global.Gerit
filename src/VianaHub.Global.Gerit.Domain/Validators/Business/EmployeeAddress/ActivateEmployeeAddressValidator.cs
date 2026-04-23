using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.EmployeeAddress;

/// <summary>
/// Validador para ativa��o de EmployeeAddress
/// </summary>
public class ActivateEmployeeAddressValidator : AbstractValidator<EmployeeAddressEntity>
{
    private readonly ILocalizationService _localization;

    public ActivateEmployeeAddressValidator(ILocalizationService localization)
    {
        _localization = localization;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.IdRequired"));

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.ModifiedByRequired"));

        RuleFor(x => x.IsDeleted)
            .Equal(false)
            .WithMessage(_localization.GetMessage("Domain.EmployeeAddress.CannotActivateDeleted"));
    }
}
