using HRManagement.Domain.Entities;

namespace HRManagement.Application.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<List<Permission>> GetByIdsAsync(
            List<int> ids, CancellationToken cancellationToken = default);
    }
}
