using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.VisitTeam;

public class VisitTeamValidator : BaseEntityValidator<VisitTeamEntity>
{
    public VisitTeamValidator(ILocalizationService localization) : base(localization)
    {
    }

    public override Task<ValidationResult> ValidateForCreateAsync(VisitTeamEntity entity)
    {
        var validator = new CreateVisitTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForUpdateAsync(VisitTeamEntity entity)
    {
        var validator = new UpdateVisitTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForActivateAsync(VisitTeamEntity entity)
    {
        var validator = new ActivateVisitTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeactivateAsync(VisitTeamEntity entity)
    {
        var validator = new DeactivateVisitTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForDeleteAsync(VisitTeamEntity entity)
    {
        var validator = new DeleteVisitTeamValidator(_localization);
        return validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(VisitTeamEntity entity)
    {
        // VisitTeam doesn't support revoke
        return Task.FromResult(new ValidationResult());
    }
}
