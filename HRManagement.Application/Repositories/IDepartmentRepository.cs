using HRManagement.Domain.Entities;

namespace HRManagement.Application.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IReadOnlyList<Department>> GetAllAsync(
            CancellationToken cancellation = default);

        Task<Department?> GetById(
            int id,
            bool trackchanges = false,
            CancellationToken cancellation = default);

        Task<bool> NameExistsAsync(string name,CancellationToken cancellation=default);

        Task AddAsync(Department department, CancellationToken cancellation = default);

        Task<int> SaveChangeAsync(CancellationToken cancellation = default);

    }
}
