namespace HRManagement.Application.Services.JobTitles
{
    public interface IJobTitleService
    {
        Task<IReadOnlyList<JobTitleResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<JobTitleResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Result<JobTitleResponse>> CreateAsync(CreateJobTitleRequest request,
            CancellationToken cancellationToken = default);
        Task<Result<JobTitleResponse>>UpdateAsync(int id,
            UpdateJobTitleRequest request , CancellationToken cancellationToken = default);

        Task<Result<bool>> DeactivateAsync(int id, CancellationToken cancellation = default);

    }
}
