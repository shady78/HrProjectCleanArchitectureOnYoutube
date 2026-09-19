namespace HRManagement.Application.Repositories
{
    public interface IRolePermissionsRepository
    {
        Task AddPermissionsToRoleAsync(
            string roleId,
            List<int> permissionIds,
            CancellationToken cancellationToken = default);

        Task ReplacePermissionsForRoleAsync(
            string roleId,
            List<int> permissionIds,
            CancellationToken cancellationToken = default);
     }
}
