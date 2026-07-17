using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;


namespace VianaHub.Global.Gerit.Domain.Validators.Business.Function;

public class VisitTeamFunctionValidator : BaseEntityValidator<VisitTeamFunctionEntity>
{
    private readonly ILocalizationService _localization;

    public VisitTeamFunctionValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(VisitTeamFunctionEntity entity)
    {
        var validator = new CreateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(VisitTeamFunctionEntity entity)
    {
        var validator = new UpdateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(VisitTeamFunctionEntity entity)
    {
        var validator = new ActivateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamFunctionEntity entity)
    {
        var validator = new DeactivateVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(VisitTeamFunctionEntity entity)
    {
        var validator = new DeleteVisitTeamFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitTeamFunctionEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}

