using HRManagement.Application.Common.Interfaces;
using HRManagement.Application.Repositories;
using HRManagement.Domain.Entities.Identity;
using HRManagement.Infrastructure.Persistence;
using HRManagement.Infrastructure.Repositories;
using HRManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,string connectionString,IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
                configuration.GetSection(nameof(JwtSettings)));

            services.AddScoped<AuditSaveChangesInterceptor>();
            services.AddDbContext<ApplicationDbContext>((serviceProvider,options) =>
            {
                options.UseSqlServer(connectionString);

                options.AddInterceptors(
                    serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
            });
           services.AddScoped<ITokenService, TokenService>();

            services
                .AddIdentity<ApplicaitonUser, IdentityRole>(options =>
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

            services.AddScoped<IDepartmentRepository , DepartmentRepository>();
            services.AddScoped<IJobTitleRepository , JobTitleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
