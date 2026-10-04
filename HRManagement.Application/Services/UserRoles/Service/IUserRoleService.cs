using HRManagement.Application.DTOs.UserRoles;

namespace HRManagement.Application.Services.UserRoles.Service
{
    public interface IUserRoleService
    {
        Task<UserRolesResponse?> GetUserRolesAsync(
            string userId,
            CancellationToken cancellationToken = default);
        Task<UserRolesResponse?> AssignRoleToUserAsync(
            AssignRoleToUserRequest request,
            CancellationToken cancellationToken = default);
        Task<UserRolesResponse?> RemoveRoleFromUserAsync(
            RemoveRoleFromUserRequest request,
            CancellationToken cancellationToken = default);
    }
}
