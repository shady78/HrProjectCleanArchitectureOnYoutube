namespace HRManagement.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<List<ApplicationRole>> GetAllWithPermissionsAsync(
            CancellationToken cancellation =default);

        Task<ApplicationRole?> GetByIdWithPermissionsAsync(
            string id, CancellationToken cancellation = default);

        Task<ApplicationRole?> GetByIdAsync(string id, CancellationToken cancellation = default);
    }
}
