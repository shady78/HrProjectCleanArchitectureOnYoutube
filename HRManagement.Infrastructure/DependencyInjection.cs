using HRManagement.Application.Security.Interfaces;
using HRManagement.Infrastructure.Security;

namespace HRManagement.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection(nameof(JwtSettings)));

        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString);

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
        });
        services.AddScoped<ITokenService, TokenService>();

        services
            .AddIdentity<ApplicaitonUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IJobTitleRepository, JobTitleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRolePermissionsRepository, RolePermissionRepository>();
        services.AddScoped<IPermissionChecker,PermissionChecker>();
        return services;
    }
}
