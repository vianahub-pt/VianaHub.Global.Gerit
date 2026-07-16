using FluentValidation;
using VianaHub.Global.Gerit.Domain.Entities.Business;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Domain.Validators.Business.DocumentType;

public class UpdateDocumentTypeValidator : AbstractValidator<DocumentTypeEntity>
{
    public UpdateDocumentTypeValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.DocumentType.IdRequired"));

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Domain.DocumentType.CodeRequired"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Domain.DocumentType.CodeMaxLength", 50));

        RuleFor(x => x.ModifiedBy)
            .NotNull()
            .WithMessage(localization.GetMessage("Domain.DocumentType.ModifiedByRequired"))
            .GreaterThan(0)
            .WithMessage(localization.GetMessage("Domain.DocumentType.ModifiedByRequired"));
    }
}
