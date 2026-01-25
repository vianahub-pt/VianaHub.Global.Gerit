namespace VianaHub.Global.Gerit.Domain.Interfaces;

public interface IEmailSender
{
    Task SendAsync(string to, string subjectKey, string bodyKey, params object[] args);
}
