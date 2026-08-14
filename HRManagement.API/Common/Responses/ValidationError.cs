namespace HRManagement.API.Common.Responses
{
    public sealed record ValidationError(
    string Field,
    string[] Messages);
}
