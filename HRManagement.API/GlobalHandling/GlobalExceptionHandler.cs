using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace HRManagement.API.GlobalHandling
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var traceId = Activity.Current.Id ?? httpContext.TraceIdentifier;

            logger.LogError(
                exception,
                "Unhandled exception. TraceId: {TraceId}, Method: {Method}," +
                "Path: {Path}",
                traceId, httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode =
                StatusCodes.Status500InternalServerError;
            var response = ApiResponse<object?>.Failed(
                "An unexpected error occured.",
                new[]
                {
                new ApiError("Server.Unexpected","Please contact support with the trace Id.",
                  traceId)
                });

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true; 
        }
    }
}
