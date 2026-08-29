namespace HRManagement.API.Common.Responses
{
    public record ApiError(string Code, string Message, string TraceId);
}
