using FluentValidation;
using VianaHub.Global.Gerit.Application.Dtos.Request.Business.DocumentType;
using VianaHub.Global.Gerit.Domain.Interfaces.Base;

namespace VianaHub.Global.Gerit.Api.Validators.Business.DocumentType;

/// <summary>
/// Validador de rota para criação de DocumentType.
/// </summary>
public class CreateDocumentTypeRouteValidator : AbstractValidator<CreateDocumentTypeRequest>
{
    public CreateDocumentTypeRouteValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localization.GetMessage("Api.Validator.DocumentType.Create.Code"))
            .MaximumLength(50)
            .WithMessage(localization.GetMessage("Api.Validator.DocumentType.Create.Code.MaximumLength", 50));
    }
}
