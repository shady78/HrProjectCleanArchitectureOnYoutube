using HRManagement.Application.Common.Exceptions;
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
            var (statusCode, code, message) = exception switch
            {
                UnauthorizedException => (
                StatusCodes.Status401Unauthorized,
                "Auth.InvalidCredencials",
                exception.Message),

                _ => (
                StatusCodes.Status500InternalServerError,
                "Server.Unexpected",
                exception.Message)
            };
            logger.LogError(
                exception,
                "Unhandled exception. TraceId: {TraceId}, Method: {Method}," +
                "Path: {Path}",
                traceId, httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode = statusCode;
            var response = ApiResponse<object?>.Failed(
                message,
                new[]
                {
                new ApiError(code,message,traceId)
                });

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
