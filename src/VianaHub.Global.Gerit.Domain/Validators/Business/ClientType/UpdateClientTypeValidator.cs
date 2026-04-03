using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.ClientType;

public class UpdateClientTypeValidator : AbstractValidator<ClientTypeEntity>
{
    public UpdateClientTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientType.IdRequired"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientType.NameRequired"))
            .MaximumLength(200)
            .WithMessage(localization.GetMessage("Domain.ClientType.NameMaxLength", 200));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.ClientType.DescriptionRequired"))
            .MaximumLength(500)
            .WithMessage(localization.GetMessage("Domain.ClientType.DescriptionMaxLength", 500));

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.ClientType.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.ClientType.ModifiedByRequired"));
    }
}
