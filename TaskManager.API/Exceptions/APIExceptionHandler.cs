using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Exceptions;

namespace TaskManager.API.ExceptionHandler;

public class APIExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails details;

        switch (exception)
        {
            case ArgumentNullException argumentNullException:
                details = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = argumentNullException.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;

            case NotFoundException notFoundException:
                details = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = notFoundException.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                break;

            default:
                details = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = exception.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

        return true;
    }
}
