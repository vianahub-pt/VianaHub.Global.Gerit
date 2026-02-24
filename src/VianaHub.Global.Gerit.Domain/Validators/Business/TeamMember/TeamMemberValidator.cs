using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.TeamMember;

public class TeamMemberValidator : BaseEntityValidator<TeamMemberEntity>
{
    private readonly ILocalizationService _localization;

    public TeamMemberValidator(ILocalizationService localization) : base(localization)
    {
        _localization = localization;
    }

    public override async Task<ValidationResult> ValidateForCreateAsync(TeamMemberEntity entity)
    {
        var validator = new CreateTeamMemberValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForUpdateAsync(TeamMemberEntity entity)
    {
        var validator = new UpdateTeamMemberValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForActivateAsync(TeamMemberEntity entity)
    {
        var validator = new ActivateTeamMemberValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeactivateAsync(TeamMemberEntity entity)
    {
        var validator = new DeactivateTeamMemberValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override async Task<ValidationResult> ValidateForDeleteAsync(TeamMemberEntity entity)
    {
        var validator = new DeleteTeamMemberValidator(_localization);
        return await validator.ValidateAsync(entity);
    }

    public override Task<ValidationResult> ValidateForRevokeAsync(TeamMemberEntity entity)
    {
        return Task.FromResult(new ValidationResult());
    }
}
