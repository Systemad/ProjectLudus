using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Exceptions;

public class GlobalExceptionsHandler(IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var problemDetails = new ProblemDetails
        {
            Status = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                //ArgumentException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            },
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = exception.Message,
        };

        //await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        //return true;
        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext()
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
            }
        );
    }
}
