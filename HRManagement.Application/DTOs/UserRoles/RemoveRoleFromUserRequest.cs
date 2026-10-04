using System;
using System.Collections.Generic;
using System.Text;

namespace HRManagement.Application.DTOs.UserRoles
{
    public class RemoveRoleFromUserRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
