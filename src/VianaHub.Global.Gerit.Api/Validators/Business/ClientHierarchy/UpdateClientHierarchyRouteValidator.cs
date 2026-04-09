using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

namespace VianaHub.Global.Gerit.Api.Validators.Business.ClientHierarchy;

public class UpdateClientHierarchyRouteValidator : AbstractValidator<UpdateClientHierarchyRequest>
{
    public UpdateClientHierarchyRouteValidator()
    {
        RuleFor(x => x.RelationshipType)
            .InclusiveBetween(1, 2).WithMessage("RelationshipType_Invalid");
    }
}
