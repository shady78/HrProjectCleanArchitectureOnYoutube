using HRManagement.Application.DTOs.UserRoles;

namespace HRManagement.Application.Services.UserRoles.Service
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicaitonUser> _userManger;
        private readonly RoleManager<ApplicationRole> _roleManger;
        public UserRoleService(UserManager<ApplicaitonUser> userManger, RoleManager<ApplicationRole> roleManger)
        {
            _userManger = userManger;
            _roleManger = roleManger;
        }

        public async Task<UserRolesResponse?> GetUserRolesAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user is null)
            {
                return null;
            }
            return await MapToUserRolesResponseAsync(user);
        }

        public async Task<UserRolesResponse?> AssignRoleToUserAsync(
            AssignRoleToUserRequest request, 
            CancellationToken cancellationToken = default)
        {
            var user = await _userManger.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return null;
            }
            var roleExists = await _roleManger.RoleExistsAsync(request.RoleName);

            if (!roleExists)
            {
                throw new Exception("Role Not Found");
            }
            var alreadyInRole = await _userManger.IsInRoleAsync(user, request.RoleName);
            if (alreadyInRole)
            {
                return await MapToUserRolesResponseAsync(user);
            }
            var result = await _userManger.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new Exception(errors);
            }
            return await MapToUserRolesResponseAsync(user);

        }

        public async Task<UserRolesResponse?> RemoveRoleFromUserAsync(
            RemoveRoleFromUserRequest request, 
            CancellationToken cancellationToken = default)
        {
            var user = await _userManger.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return null;
            }
            var isInRole = await _userManger.IsInRoleAsync(user, request.RoleName);
            if (!isInRole)
            {
                return await MapToUserRolesResponseAsync(user);
            }
            var result = await _userManger.RemoveFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new Exception(errors);
            }
            return await MapToUserRolesResponseAsync(user);
        }

        // helper Method
        private async Task<UserRolesResponse> MapToUserRolesResponseAsync(ApplicaitonUser user)
        {
            var roles = await _userManger.GetRolesAsync(user);

            return new UserRolesResponse
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = roles.ToList()
            };
        }
    }
}
