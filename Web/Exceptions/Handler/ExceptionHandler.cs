using System.Net;
using FluentValidation;

namespace Web.Exceptions.Handler;

public class ExceptionHandler
{
    public static (HttpStatusCode Code, object Body) Handle(Exception exception, HttpContext context)
    {
        var defaultKey = exception.GetType().Name;

        return exception switch
        {
            ValidationException validationException => (HttpStatusCode.UnprocessableEntity,
                                                        ValidationExceptionDto.From(validationException)),

            RestException restException => (restException.Code,
                                            new ExceptionDto
                                            {
                                                Key = restException.Type,
                                                Message = exception.Message,
                                            }),

            UnauthorizedException unauthorizedException => (HttpStatusCode.Unauthorized,
                                                            new ExceptionDto { Key = unauthorizedException.Error.ToString() }),

            NotImplementedException => (HttpStatusCode.NotImplemented,
                                        new ExceptionDto
                                        {
                                            Key = defaultKey,
                                            Message = @"Метод еще не реализован ¯\_(ツ)_/¯"
                                        }),

            _ => (HttpStatusCode.InternalServerError,
                  new ExceptionDto
                  {
                      Key = defaultKey,
                      Message = exception.Message,
                  })
        };
    }
}