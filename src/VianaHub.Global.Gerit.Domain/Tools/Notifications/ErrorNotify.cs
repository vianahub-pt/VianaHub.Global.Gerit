using System.Net;

namespace VianaHub.Global.Gerit.Domain.Tools.Notifications;

public class ErrorNotify
{
    public HttpStatusCode StatusCode { get; set; }
    public List<string> Message { get; set; } = [];

    public void Add(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Message.Add(message);
        StatusCode = statusCode;
    }
}
