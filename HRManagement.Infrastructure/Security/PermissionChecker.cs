using HRManagement.Application.Security.Interfaces;

namespace HRManagement.Infrastructure.Security
{
    public class PermissionChecker
        (ApplicationDbContext _context,
        UserManager<ApplicaitonUser> _userManager): IPermissionChecker
    {
        public async Task<bool> HasPermissionAsync(
            string userId,
            string permission,
            CancellationToken cancellation)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;
            }
            var roleNames = await _userManager.GetRolesAsync(user);

            if (!roleNames.Any())
            {
                return false;
            }
            return await _context.RolePermissions
                .AsNoTracking()
                .AnyAsync(
                    rolePermissions => roleNames.Contains(rolePermissions.Role.Name!) &&
                    rolePermissions.Permission.Name == permission,
                    cancellation);
        }
    }
}
