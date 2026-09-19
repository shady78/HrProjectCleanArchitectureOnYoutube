namespace HRManagement.Infrastructure.Repositories
{
    public class RolePermissionRepository(
        ApplicationDbContext _context) : IRolePermissionsRepository
    {
        public async Task AddPermissionsToRoleAsync(string roleId, List<int> permissionIds, CancellationToken cancellationToken = default)
        {
            var validPermissionIds = await _context.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            foreach (var permissionId in validPermissionIds.Distinct())
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ReplacePermissionsForRoleAsync(string roleId, List<int> permissionIds, CancellationToken cancellationToken = default)
        {
            var existingRolePermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == roleId);
            _context.RolePermissions.RemoveRange(existingRolePermissions);

            var validPermissionIds = await _context.Permissions
              .Where(p => permissionIds.Contains(p.Id))
              .Select(p => p.Id)
              .ToListAsync();

            foreach (var permissionId in validPermissionIds.Distinct())
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
