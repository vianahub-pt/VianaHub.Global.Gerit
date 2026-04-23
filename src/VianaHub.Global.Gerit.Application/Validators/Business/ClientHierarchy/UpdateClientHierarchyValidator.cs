using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.ClientHierarchy;

namespace VianaHub.Global.Gerit.Application.Validators.Business.ClientHierarchy;

public class UpdateClientHierarchyValidator : AbstractValidator<UpdateClientHierarchyRequest>
{
    public UpdateClientHierarchyValidator()
    {
        RuleFor(x => x.RelationshipType)
            .InclusiveBetween(1, 2).WithMessage("RelationshipType_Invalid");
    }
}
