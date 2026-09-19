namespace HRManagement.Infrastructure.Repositories
{
    public class RoleRepository(ApplicationDbContext _context) : IRoleRepository
    {
        public async Task<List<ApplicationRole>> GetAllWithPermissionsAsync(
            CancellationToken cancellation = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(role => role.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync(cancellation);
        }
        public async Task<ApplicationRole?> GetByIdWithPermissionsAsync(string id, CancellationToken cancellation = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(role => role.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(role => role.Id == id,cancellation);
        }

        public async Task<ApplicationRole?> GetByIdAsync(string id, CancellationToken cancellation = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(role => role.Id == id, cancellation);
        }

    }
}
