using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable(nameof(Attendance));
            builder.HasKey(c => c.AttendanceId);
            builder.Property(c => c.DateTime)
                .HasColumnType("date");
            builder.Property(c => c.Status)
                .HasColumnType("varchar(150)");

            //Foreign Key
            builder.HasOne(c => c.StudentLink)
                .WithMany(c => c.AttendanceList)
                .HasForeignKey(c => c.StudentLRN);

        }
    }








}
