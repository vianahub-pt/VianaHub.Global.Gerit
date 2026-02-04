using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces;


namespace VianaHub.Global.Gerit.Domain.Validators.Business.Function;

public class FunctionValidator : BaseEntityValidator<FunctionEntity>
{
    private readonly ILocalizationService _localization;

    public FunctionValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(FunctionEntity entity)
    {
        var validator = new CreateFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(FunctionEntity entity)
    {
        var validator = new UpdateFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(FunctionEntity entity)
    {
        var validator = new ActivateFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(FunctionEntity entity)
    {
        var validator = new DeactivateFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(FunctionEntity entity)
    {
        var validator = new DeleteFunctionValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(FunctionEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}

