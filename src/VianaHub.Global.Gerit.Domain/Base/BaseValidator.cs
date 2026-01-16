using FluentValidation;
using VianaHub.Global.Gerit.Domain.Enums;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Domain.Base;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected readonly INotify Notify;

    protected BaseValidator(INotify notify)
    {
        Notify = notify;
    }

    public abstract void Validate(T model, ServiceContext context = ServiceContext.OutroContexto);
    public abstract void ValidateAll(T model);

    protected void NotifyErrors(FluentValidation.Results.ValidationResult result)
    {
        foreach (var error in result.Errors)
            Notify.Add(error.ErrorMessage, 400);
    }
}
