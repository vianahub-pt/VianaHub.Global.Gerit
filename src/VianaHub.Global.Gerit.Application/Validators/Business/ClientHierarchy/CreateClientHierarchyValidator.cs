using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientHierarchy;

public class CreateClientHierarchyValidator : AbstractValidator<CreateClientHierarchyRequest>
{
    public CreateClientHierarchyValidator()
    {
        RuleFor(x => x.ParentId)
            .GreaterThan(0).WithMessage("ParentClientId_Required");

        RuleFor(x => x.ChildId)
            .GreaterThan(0).WithMessage("ChildClientId_Required");

        RuleFor(x => x.RelationshipType)
            .InclusiveBetween(1, 2).WithMessage("RelationshipType_Invalid");

        RuleFor(x => x)
            .Must(x => x.ParentId != x.ChildId)
            .WithMessage("client_hierarchy.same_client");
    }
}
