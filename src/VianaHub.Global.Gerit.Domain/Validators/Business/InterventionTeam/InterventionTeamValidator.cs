using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.InterventionTeam;

public class InterventionTeamValidator : BaseEntityValidator<InterventionTeamEntity>
{
    public InterventionTeamValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(InterventionTeamEntity entity)
    {
        var validator = new CreateInterventionTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(InterventionTeamEntity entity)
    {
        var validator = new UpdateInterventionTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(InterventionTeamEntity entity)
    {
        var validator = new ActivateInterventionTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(InterventionTeamEntity entity)
    {
        var validator = new DeactivateInterventionTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(InterventionTeamEntity entity)
    {
        var validator = new DeleteInterventionTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(InterventionTeamEntity entity)
    {
        // InterventionTeam doesn't support revoke
        return Task.FromResult(new ValidationResult());
    }
}
