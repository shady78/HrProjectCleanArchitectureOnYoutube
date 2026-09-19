using HRManagement.Application.DTOs.Roles;
using System.Globalization;

namespace HRManagement.Application.Services.Roles.Services
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<RoleResponse?> GetByIdAsync(
            string id, CancellationToken cancellationToken = default);

        Task<RoleResponse?> CreateAsync(
            CreateRoleRequest request, CancellationToken cancellationToken = default);

        Task<RoleResponse?> UpdatePermissionsAsync(
            string roleId,
            UpdateRolePermissionsRequest request, CancellationToken cancellationToken = default);
        
        Task<List<PermissionResponse>> GetAllPermissionsAsync(
            CancellationToken cancellation = default);
    }
}
