using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientHierarchy;

public class DeleteClientHierarchyValidator : AbstractValidator<ClientHierarchyEntity>
{
    public DeleteClientHierarchyValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientHierarchy_AlreadyDeleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
