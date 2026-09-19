using Microsoft.AspNetCore.Authorization;

namespace HRManagement.API.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get;  }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
