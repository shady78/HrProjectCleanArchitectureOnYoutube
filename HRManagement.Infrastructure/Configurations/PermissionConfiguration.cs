using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagement.Infrastructure.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(permission => permission.Description)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(permission => permission.Module)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(permission => permission.Name)
                .IsUnique();

            //builder.HasData(
            //    new Permission
            //    {
            //        Id = 1,
            //        Description = "can view Employees",
            //        Name = "Employees.View",
            //        Module = "Employees"
            //    });
        }
    }
}
