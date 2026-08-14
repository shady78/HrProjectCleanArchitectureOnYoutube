using FluentValidation;
using HRManagement.Application.Services.Departments;
using HRManagement.Application.Services.JobTitles;
using HRManagement.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Application
{
    public static class DependencyInjection
    { 
        // builder.Services.AddApplication();
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddValidatorsFromAssemblyContaining
                <CreateDepartmentValidator>();
            return services;
        }
    }
}
