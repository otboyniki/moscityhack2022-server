using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Web.Exceptions.Handler;

namespace Web.Extensions;

public static class ExceptionHandlerExtensions
{
    public static void UseApiExceptionHandler(this IApplicationBuilder app)
    {
        var logger = app.ApplicationServices
                        .GetRequiredService<ILogger<ExceptionHandler>>();

        app.UseExceptionHandler(pipeline =>
        {
            pipeline.Run(context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature == null)
                {
                    return Task.CompletedTask;
                }

                try
                {
                    var (code, response) = ExceptionHandler.Handle(feature.Error, context);

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Response.StatusCode = (int)code;
                    return context.Response.WriteAsJsonAsync(response);
                }
                catch (Exception e)
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    return context.Response.WriteAsJsonAsync(new ExceptionDto
                    {
                        Key = e.GetType().Name,
                        Message = !string.IsNullOrEmpty(e.Message)
                            ? e.Message
                            : "Что-то пошло не так",
                    });
                }
                finally
                {
                    if (context.Response.StatusCode == 500)
                    {
                        logger.LogError(feature.Error,
                            "An unhandled exception has occurred while executing the request.");
                    }
                }
            });
        });
    }
}