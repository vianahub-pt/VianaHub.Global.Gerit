using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientHierarchy;

public class CreateClientHierarchyRouteValidator : AbstractValidator<CreateClientHierarchyRequest>
{
    public CreateClientHierarchyRouteValidator()
    {
        RuleFor(x => x.ParentId)
            .GreaterThan(0).WithMessage("ParentClientId_Required");

        RuleFor(x => x.ChildId)
            .GreaterThan(0).WithMessage("ChildClientId_Required");

        RuleFor(x => x.RelationshipType)
            .InclusiveBetween(1, 2).WithMessage("RelationshipType_Invalid");
    }
}
