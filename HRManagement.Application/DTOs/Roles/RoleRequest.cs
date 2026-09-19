namespace HRManagement.Application.DTOs.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
    public class UpdateRolePermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new();
    }
    public class PermissionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public string Module { get; set; }= string.Empty;
    }
    public class RoleResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PermissionResponse> Permissions { get; set; } = new();
    }
}
