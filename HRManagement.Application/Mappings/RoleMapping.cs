using HRManagement.Application.DTOs.Roles;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Mappings
{
    public static class RoleMapping
    {
        public static PermissionResponse ToResponse(this Permission permission)
        {
            return new PermissionResponse
            {
                Id = permission.Id,
                Description = permission.Description,
                Module = permission.Module,
                Name = permission.Name
            };
        }
        public static RoleResponse ToResponse(this ApplicationRole role)
        {
            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Description = role.Description,
                Permissions = role.RolePermissions
                    .Select(rp => rp.Permission.ToResponse())
                    .ToList()
            };
        }
    }
}
