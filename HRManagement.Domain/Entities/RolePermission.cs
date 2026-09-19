using HRManagement.Domain.Entities.Identity;

namespace HRManagement.Domain.Entities
{
    public class RolePermission
    {
        public string RoleId { get; set; } = string.Empty;
        public ApplicationRole Role { get; set; } = default!;
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
