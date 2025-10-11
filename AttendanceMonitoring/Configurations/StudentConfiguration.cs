using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(nameof(Student));
            builder.HasKey(c => c.StudentId);
            builder.Property(c => c.FirstName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.LastName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.LRN)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.PhoneNumber)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.EnrollmentStatus)
                .HasColumnType("varchar(150)");

        }
    }








}
