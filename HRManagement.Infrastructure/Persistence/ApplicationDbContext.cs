using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HRManagement.Domain.Entities.Identity;
namespace HRManagement.Infrastructure.Persistence
{
    public sealed class ApplicationDbContext
        (DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicaitonUser>(options)
    {

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<JobTitle> JobTitles => Set<JobTitle>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
