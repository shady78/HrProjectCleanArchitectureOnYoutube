using HRManagement.Application.Repositories;
using HRManagement.Infrastructure.Persistence;
using HRManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IDepartmentRepository , DepartmentRepository>();
            services.AddScoped<IJobTitleRepository , JobTitleRepository>();

            return services;
        }
    }
}
