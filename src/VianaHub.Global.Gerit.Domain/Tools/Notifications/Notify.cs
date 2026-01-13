using System.Net;

namespace VianaHub.Global.Gerit.Domain.Tools.Notifications;

public class Notify : INotify
{
    #region Properties
    private ErrorNotify Notifications { get; set; } = new ErrorNotify();

    #endregion

    #region Public Methods

    public void Add(string message, int statusCode = 400)
    {
        HttpStatusCode httpStatusCode = (HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), statusCode);

        Notifications.Add(message, httpStatusCode);
    }
    public bool HasNotify() => Notifications.Message.Count != 0;
    public List<string> GetErrorMessage()
    {
        return Notifications.Message;
    }
    public HttpStatusCode GetStatusCode()
    {
        return !HasNotify() ? HttpStatusCode.OK : Notifications.StatusCode;
    }

    #endregion
}
