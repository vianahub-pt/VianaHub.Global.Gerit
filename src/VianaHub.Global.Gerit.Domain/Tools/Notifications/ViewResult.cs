using System.Net;

namespace VianaHub.Global.Gerit.Domain.Tools.Notifications;

public class ViewResult
{
    public object Response { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public ViewResult(object response, HttpStatusCode statusCode)
    {
        Response = response;
        StatusCode = statusCode;
    }
}

