using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientHierarchy;

public class UpdateClientHierarchyValidator : AbstractValidator<ClientHierarchyEntity>
{
    public UpdateClientHierarchyValidator()
    {
        Include(new ClientHierarchyValidator());

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy_Required");
    }
}
