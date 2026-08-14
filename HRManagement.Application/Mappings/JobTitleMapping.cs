using HRManagement.Application.DTOs.JobTitles;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Mappings
{
    public static class JobTitleMapping
    {
        public static JobTitle ToEntity(this CreateJobTitleRequest request)
        {
            return new JobTitle
            {
                Title = request.Title.Trim(),
                Description = NormalizeDescription(request.Description),
            };
        }
        public static void MapTo(this UpdateJobTitleRequest request, JobTitle jobTitle)
        {
            jobTitle.Title = request.Title.Trim();
            jobTitle.Description = NormalizeDescription(request.Description);
        }

        public static JobTitleResponse ToResponse(this JobTitle jobTitle)
        {
            return new JobTitleResponse(
                jobTitle.Id, jobTitle.Title, jobTitle.Description, jobTitle.IsActive);
        }
        private static string? NormalizeDescription(string? description)
        {
            return string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }
    }
}
