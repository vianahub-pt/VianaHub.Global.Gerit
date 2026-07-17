using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;


namespace VianaHub.Global.Gerit.Domain.Validators.Business.Function;

public class VisitTeamFunctionValidator : BaseEntityValidator<VisitTeamFunctionsEntity>
{
    private readonly ILocalizationService _localization;

    public VisitTeamFunctionValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitTeamFunctionsEntity entity)
    {
        var validator = new CreateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitTeamFunctionsEntity entity)
    {
        var validator = new UpdateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitTeamFunctionsEntity entity)
    {
        var validator = new ActivateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamFunctionsEntity entity)
    {
        var validator = new DeactivateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(VisitTeamFunctionsEntity entity)
    {
        var validator = new DeleteVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitTeamFunctionsEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}

