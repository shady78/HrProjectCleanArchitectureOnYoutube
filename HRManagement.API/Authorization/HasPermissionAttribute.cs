using Microsoft.AspNetCore.Authorization;

namespace HRManagement.API.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        //[HasPermission(SystemPermissions.Departments.View)]
        //[Authorze(Policy="Permission:Derpartment.View")]

        public HasPermissionAttribute(string permission)
        {
            Policy = $"{PermissionAuthorizationPolicyProvider.PolicyPrefix}" +
                $"{permission}";
        }
    }
}
