using HRManagement.Application.DTOs.UserRoles;
using HRManagement.Application.Services.UserRoles.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController(IUserRoleService _userRoleService) : ControllerBase
    {

        [HttpGet("{userId}")]
        [HasPermission(SystemPermissions.Roles.RolesView)]
        public async Task<IActionResult> Get(string userId,
            CancellationToken cancellationToken)
        {
            var response = await _userRoleService.GetUserRolesAsync(userId,cancellationToken);
            if (response is null)
            {
                return NotFound(ApiResponse<UserRolesResponse>.Failed("user not found"));
            }
            return Ok(ApiResponse<UserRolesResponse>
                .Succeeded(response,"User roles retirved successfully."));
        }
        /*{
  "userId": "66bc175c-3f2b-4209-acdd-3c37c03159d9",
  "roleName": "role2"
        eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NmJjMTc1Yy0zZjJiLTQyMDktYWNkZC0zYzM3YzAzMTU5ZDkiLCJlbWFpbCI6InRlc3RAZ21haWwuY29tIiwianRpIjoiM2UyNTdkOWItOTQ5Zi00NDM4LWI0MDAtZmM1MjA3ODM0NDNlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI2NmJjMTc1Yy0zZjJiLTQyMDktYWNkZC0zYzM3YzAzMTU5ZDkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ0ZXN0IHN0cmluZyIsImV4cCI6MTc5MTEyMDAxNywiaXNzIjoiSFJNYW5hZ2VtZW50LkFQSSIsImF1ZCI6IkhSTWFuYWdlbWVudFN5c3RlbVVzZXIifQ.mjd4-krc8ZeFWZl7NUQto14kf4ANSwQh7fERrGbHBiU

          "email": "test@gmail.com",
  "password": "Test@123",
}*/
        [HttpPost("assign")]
        //[HasPermission(SystemPermissions.Roles.RolesManagePermissions)]
        public async Task<IActionResult> Post(AssignRoleToUserRequest request,CancellationToken cancellationToken)
        {
            var response = await _userRoleService.AssignRoleToUserAsync(request, cancellationToken);
            if (response is null)
            {
                return NotFound(ApiResponse<UserRolesResponse>.Failed("user not found"));
            }
            return Ok(ApiResponse<UserRolesResponse>
                .Succeeded(response, "Role assigned to user successfully."));
        }
        [HttpPost("remove")]
        [HasPermission(SystemPermissions.Roles.RolesManagePermissions)]
        public async Task<IActionResult> Remove(RemoveRoleFromUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _userRoleService.RemoveRoleFromUserAsync(request, cancellationToken);
            if (response is null)
            {
                return NotFound(ApiResponse<UserRolesResponse>.Failed("user not found"));
            }
            return Ok(ApiResponse<UserRolesResponse>
                .Succeeded(response, "Role removed from user successfully."));
        }
    }
}
