using System.Net;

namespace VianaHub.Global.Gerit.Domain.Tools.Notifications;

public interface INotify
{
    void Add(string message, int statusCode = 400);
    bool HasNotify();
    List<string> GetErrorMessage();
    HttpStatusCode GetStatusCode();
}
