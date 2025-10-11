using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable(nameof(Parent));
            builder.HasKey(c => c.ParentId);
            builder.Property(c => c.FirstName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.LastName)
               .HasColumnType("varchar(150)");
            builder.Property(c => c.Email)
                .HasColumnType("varchar(150)");

        }
    }








}
