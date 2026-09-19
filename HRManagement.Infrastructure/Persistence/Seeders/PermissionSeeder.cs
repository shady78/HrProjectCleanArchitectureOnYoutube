namespace HRManagement.Infrastructure.Persistence.Seeders
{
    public static class PermissionSeeder
    {
        // Add and Update permissions by name , module , description
        public static async Task SeedAsync(ApplicationDbContext _context)
        {
            var existingPermissionNames = await _context.Permissions
                .AsNoTracking()
                .Select(p => p.Name)
                .ToListAsync();

            var newPermissions = SystemPermissions.All
                .Where(permissionDefinition =>
             !existingPermissionNames.Contains(permissionDefinition.Name))
                .Select(permissionDefinition => new Permission
                {
                    Name = permissionDefinition.Name,
                    Description = permissionDefinition.Description,
                    Module = permissionDefinition.Module
                }).ToList();

            if (!newPermissions.Any())
            {
                return;
            }
            await _context.Permissions.AddRangeAsync(newPermissions);
            await _context.SaveChangesAsync();
        }
    }
}
