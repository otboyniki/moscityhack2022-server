using System.Net;

namespace Web.Exceptions;

public class RestException : Exception
{
    public HttpStatusCode Code { get; }
    public string Type { get; }

    public RestException(string message, HttpStatusCode code)
        : base(message) =>
        (Code, Type) = (code, code.ToString());

    public RestException(string message, HttpStatusCode code, Exception innerException)
        : base(message,
            innerException) =>
        (Code, Type) = (code, code.ToString());

    public RestException(string message, string type, HttpStatusCode code)
        : base(message) =>
        (Code, Type) = (code, type);

    public RestException(string message, string type, HttpStatusCode code, Exception innerException)
        : base(message,
            innerException) =>
        (Code, Type) = (code, type);
}