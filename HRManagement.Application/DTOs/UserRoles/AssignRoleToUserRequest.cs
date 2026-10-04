namespace HRManagement.Application.DTOs.UserRoles
{
    public class AssignRoleToUserRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
