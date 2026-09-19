namespace HRManagement.Application;

public static class DependencyInjection
{
    // builder.Services.AddApplication();
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IJobTitleService, JobTitleService>();
        services.AddValidatorsFromAssemblyContaining
            <CreateDepartmentValidator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}

