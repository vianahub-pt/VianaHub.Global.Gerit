using FluentValidation;
using FluentValidation.Results;
using VianaHub.Global.Gerit.Domain.Base;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientHierarchy;

public class ClientHierarchyValidator : AbstractValidator<ClientHierarchyEntity>, IEntityDomainValidator<ClientHierarchyEntity>
{
    public ClientHierarchyValidator()
    {
        RuleFor(x => x.ParentId)
            .GreaterThan(0).WithMessage("ParentClientId_Required")
            .NotEqual(x => x.ChildId).WithMessage("ParentClientId_CannotBeEqualToChildClientId");

        RuleFor(x => x.ChildId)
            .GreaterThan(0).WithMessage("ChildClientId_Required")
            .NotEqual(x => x.ParentId).WithMessage("ChildClientId_CannotBeEqualToParentClientId");
    }

    public async Task<ValidationResult> ValidateForCreateAsync(ClientHierarchyEntity entity)
        => await new CreateClientHierarchyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForUpdateAsync(ClientHierarchyEntity entity)
        => await new UpdateClientHierarchyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForActivateAsync(ClientHierarchyEntity entity)
        => await new ActivateClientHierarchyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeactivateAsync(ClientHierarchyEntity entity)
        => await new DeactivateClientHierarchyValidator().ValidateAsync(entity);

    public async Task<ValidationResult> ValidateForDeleteAsync(ClientHierarchyEntity entity)
        => await new DeleteClientHierarchyValidator().ValidateAsync(entity);

    public Task<ValidationResult> ValidateForRevokeAsync(ClientHierarchyEntity entity)
        => ValidateForDeleteAsync(entity);
}
