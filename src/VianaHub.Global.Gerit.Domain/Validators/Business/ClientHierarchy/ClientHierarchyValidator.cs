using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientHierarchy;

public class ClientHierarchyValidator : AbstractValidator<ClientHierarchyEntity>
{
    public ClientHierarchyValidator()
    {
        RuleFor(x => x.ParentClientId)
            .GreaterThan(0).WithMessage("ParentClientId_Required")
            .NotEqual(x => x.ChildClientId).WithMessage("ParentClientId_CannotBeEqualToChildClientId");

        RuleFor(x => x.ChildClientId)
            .GreaterThan(0).WithMessage("ChildClientId_Required")
            .NotEqual(x => x.ParentClientId).WithMessage("ChildClientId_CannotBeEqualToParentClientId");

        RuleFor(x => x.RelationshipType)
            .InclusiveBetween(1, 2).WithMessage("RelationshipType_Invalid");
    }
}
