using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HRManagement.API.Authorization
{
    public class PermissionAuthorizationPolicyProvider :
        DefaultAuthorizationPolicyProvider
    {
        public const string PolicyPrefix = "Permission:";
        public PermissionAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(
            string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return base.GetPolicyAsync(policyName);
            }
            var permission = policyName[PolicyPrefix.Length..];

            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}
