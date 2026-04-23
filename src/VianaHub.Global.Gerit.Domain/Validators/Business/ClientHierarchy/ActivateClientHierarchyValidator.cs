using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientHierarchy;

public class ActivateClientHierarchyValidator : AbstractValidator<ClientHierarchyEntity>
{
    public ActivateClientHierarchyValidator()
    {
        RuleFor(x => x.IsDeleted)
            .Equal(false).WithMessage("ClientHierarchy_Deleted");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
