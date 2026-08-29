using HRManagement.Domain.Entities;

namespace HRManagement.Application.Repositories
{
    public interface IJobTitleRepository
    {
        Task<IReadOnlyList<JobTitle>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<JobTitle?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> TitleExistsAsync(string title,CancellationToken cancellationToken = default);
        Task AddAsync(JobTitle jobTitle , CancellationToken cancellationToken = default);

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
