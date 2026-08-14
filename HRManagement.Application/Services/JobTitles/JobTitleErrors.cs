namespace HRManagement.Application.Services.JobTitles
{
    public static class JobTitleErrors
    {
        public static ServiceError NotFound(int id)
          => new(
            "JobTitles.NotFound",
            $"Job Title with Id '{id}' was not found",
            ErrorType.NotFound);

        public static ServiceError DuplicateTitle(string title)
            => new(
                "JobTitles.DuplicateTitle",
                $"A job title named '{title}' already exists.",
                ErrorType.Conflict);
    }
}
