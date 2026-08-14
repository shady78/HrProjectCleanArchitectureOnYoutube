using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagement.Infrastructure.Configurations
{
    public sealed class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.ToTable("JobTitles");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description).HasMaxLength(500);

            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasIndex(x => x.Title).IsUnique();
        }
    }
}
