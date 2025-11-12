using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class ClassAdviserConfiguration : IEntityTypeConfiguration<Class_Adviser>
    {
        public void Configure(EntityTypeBuilder<Class_Adviser> builder)
        {
            builder.ToTable(nameof(Class_Adviser));
            builder.HasKey(c => c.ClassAdviserId);
            builder.Property(c => c.FirstName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.LastName)
                .HasColumnType("varchar(150)");

        }
    }








}
