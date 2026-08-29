using HRManagement.Application.Mappings;
using HRManagement.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace HRManagement.Application.Services.JobTitles
{
    public class JobTitleService(
        IJobTitleRepository repository,
        ILogger<JobTitleService> logger) 
        : IJobTitleService
    {
        public async Task<IReadOnlyList<JobTitleResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var jobTitles = await repository.GetAllAsync(cancellationToken);
            return jobTitles.Select(j => j.ToResponse()).ToList();
        }

        public async Task<Result<JobTitleResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobTitle = await repository.GetByIdAsync(id, cancellationToken);
            return jobTitle is null ?
                Result<JobTitleResponse>.Failure(JobTitleErrors.NotFound(id))
              : Result<JobTitleResponse>.Success(jobTitle.ToResponse());
        }

        public async Task<Result<JobTitleResponse>> CreateAsync(CreateJobTitleRequest request, CancellationToken cancellationToken = default)
        {
            var normalizedTitle = request.Title.Trim();
            if (await repository.TitleExistsAsync(normalizedTitle,cancellationToken))
            {
                logger.LogWarning(
                    $"Job title creation rejected because {normalizedTitle} already exist");
                return Result<JobTitleResponse>
                    .Failure(JobTitleErrors.DuplicateTitle(normalizedTitle));
            }
            var jobTitle = request.ToEntity();
            await repository.AddAsync(jobTitle, cancellationToken);
            await repository.SaveChangeAsync(cancellationToken);
            logger.LogInformation($"Job Title {jobTitle.Id} created successfully with" +
                $"title {jobTitle.Title}");
            return Result<JobTitleResponse>.Success(jobTitle.ToResponse());
        }

        public async Task<Result<JobTitleResponse>> UpdateAsync(int id, UpdateJobTitleRequest request, CancellationToken cancellationToken = default)
        {
            var jobTitle = await repository.GetByIdAsync(id, cancellationToken);
            if (jobTitle is null)
            {
                return Result<JobTitleResponse>.Failure(JobTitleErrors.NotFound(id));
            }
            var normalizedTitle = request.Title.Trim();
            if (await repository.TitleExistsAsync(normalizedTitle, cancellationToken))
            {
                return Result<JobTitleResponse>
                    .Failure(JobTitleErrors.DuplicateTitle(normalizedTitle));
            }
            request.MapTo(jobTitle);
            await repository.SaveChangeAsync(cancellationToken);

            return Result<JobTitleResponse>.Success(jobTitle.ToResponse());
        }
        public async Task<Result<bool>> DeactivateAsync(int id, CancellationToken cancellation = default)
        {
            var jobTitle = await repository.GetByIdAsync(id, cancellation);
            if (jobTitle is null)
            {
                return Result<bool>.Failure(JobTitleErrors.NotFound(id));
            }
            if (jobTitle.IsActive)
            {
                jobTitle.IsActive = false;
                await repository.SaveChangeAsync(cancellation);
            }

            return Result<bool>.Success(true);
        }


    }
}
