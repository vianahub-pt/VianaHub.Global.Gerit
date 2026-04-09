using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientHierarchy;

public class CreateClientHierarchyValidator : AbstractValidator<CreateClientHierarchyRequest>
{
    public CreateClientHierarchyValidator()
    {
        RuleFor(x => x.ParentClientId)
            .GreaterThan(0).WithMessage("ParentClientId_Required");

        RuleFor(x => x.ChildClientId)
            .GreaterThan(0).WithMessage("ChildClientId_Required");

        RuleFor(x => x.RelationshipType)
            .InclusiveBetween(1, 2).WithMessage("RelationshipType_Invalid");

        RuleFor(x => x)
            .Must(x => x.ParentClientId != x.ChildClientId)
            .WithMessage("client_hierarchy.same_client");
    }
}
