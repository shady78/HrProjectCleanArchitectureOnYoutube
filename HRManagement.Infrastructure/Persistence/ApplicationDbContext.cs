using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Persistence
{
    public sealed class ApplicationDbContext
        (DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {


        public DbSet<Department> Departments => Set<Department>();
        public DbSet<JobTitle> JobTitles => Set<JobTitle>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
