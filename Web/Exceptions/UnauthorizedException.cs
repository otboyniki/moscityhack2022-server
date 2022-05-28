using Web.Exceptions.Models;

namespace Web.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedError Error { get; }

    public UnauthorizedException(UnauthorizedError error) => Error = error;
}