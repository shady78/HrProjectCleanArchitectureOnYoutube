using HRManagement.Application.DTOs.Roles;

namespace HRManagement.Application.Services.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionsRepository _rolePermissionRepository;
        private readonly IRoleRepository _roleRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IRolePermissionsRepository rolePermissionRepository,
            IPermissionRepository permissionRepository, 
            RoleManager<ApplicationRole> roleManager)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _roleManager = roleManager;
        }

        public async Task<List<RoleResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var roles = await _roleRepository.GetAllWithPermissionsAsync(cancellationToken);
            return roles.Select(role => role.ToResponse()).ToList();
        }
        public async Task<RoleResponse?> GetByIdAsync(
            string id, 
            CancellationToken cancellationToken = default)
        {
           var role = await _roleRepository.GetByIdWithPermissionsAsync(id,
               cancellationToken);
            if (role is null)
            {
                return null;
            }
            return role.ToResponse();
        }
        public async Task<RoleResponse?> CreateAsync(
            CreateRoleRequest request, CancellationToken cancellationToken = default)
        {
            var roleExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleExists)
            {
                throw new Exception($"Role with name: {request.Name} already exists.");
            }
            var role = new ApplicationRole
            {
                Name = request.Name,
                Description = request.Description
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(error => error.Description));
                throw new Exception(errors);
            }
            if (request.PermissionIds.Any())
            {
                await _rolePermissionRepository.AddPermissionsToRoleAsync(
                    role.Id,
                    request.PermissionIds,
                    cancellationToken);
            }
            var created = await GetByIdAsync(role.Id, cancellationToken);
            return created;
        }


        public async Task<List<PermissionResponse>> GetAllPermissionsAsync(CancellationToken cancellation = default)
        {
            var permissions = await
                _permissionRepository.GetAllAsync(cancellation);
            return permissions.Select(p => p.ToResponse()).ToList();
        }


        public async Task<RoleResponse?> UpdatePermissionsAsync(
            string roleId,
            UpdateRolePermissionsRequest request,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return null;
            }
            await _rolePermissionRepository.ReplacePermissionsForRoleAsync(
                roleId,
                request.PermissionIds.Distinct().ToList(),
                cancellationToken);
            return await GetByIdAsync(roleId, cancellationToken);
        }
    }
}
