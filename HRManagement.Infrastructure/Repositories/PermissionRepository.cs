namespace HRManagement.Infrastructure.Repositories
{
    public class PermissionRepository(ApplicationDbContext _context) : IPermissionRepository
    {
        public async Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Permissions
                   .OrderBy(p => p.Module)
                   .ThenBy(p => p.Name)
                   .ToListAsync(cancellationToken);
        }

        public async Task<List<Permission>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            return await _context.Permissions
                .Where(p => ids.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
