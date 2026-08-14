namespace HRManagement.Application.DTOs.JobTitles
{
    public record CreateJobTitleRequest(string Title, string? Description);
    public record UpdateJobTitleRequest(string Title, string? Description);
    public record JobTitleResponse(int Id, string Title, string? Description,bool IsActive);
}
