namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService _roleService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(
            CancellationToken cancellation)
        {
            var roles = await _roleService.GetAllAsync(cancellation);
            return Ok(ApiResponse<List<RoleResponse>>.Succeeded(
                roles, "Roles retrieved Successfully."));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            string id, CancellationToken cancellation)
        {
            var role = await _roleService.GetByIdAsync(id, cancellation);
            if (role is null)
            {
                return NotFound(ApiResponse<RoleResponse>.Failed(
                    "Role not found."));
            }
            return Ok(ApiResponse<RoleResponse>.Succeeded(
                role, "role retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            CreateRoleRequest request,
            CancellationToken cancellation)
        {
            var role = await _roleService.CreateAsync(request, cancellation);
            return Ok(ApiResponse<RoleResponse>.Succeeded(
                role!, "role created successfully."));
        }

        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> Put(
            string roleId,
            UpdateRolePermissionsRequest request,
            CancellationToken cancellation)
        {
            var role = await _roleService.UpdatePermissionsAsync(
                roleId, request, cancellation);
            if (role is null)
            {
                return NotFound(ApiResponse<RoleResponse>.Failed(
                  "Role not found."));
            }
            return Ok(ApiResponse<RoleResponse>.Succeeded(
                role, "role permissions updated successfully."));
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetAllPermissions(
            CancellationToken cancellation)
        {
            var permissions = await _roleService.GetAllPermissionsAsync(cancellation);

            return Ok(ApiResponse<List<PermissionResponse>>.Succeeded(
                permissions, "Permissions retrieved successfully."));
        }

    }
}
