using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class AdvisoryConfiguration : IEntityTypeConfiguration<Advisory>
    {
        public void Configure(EntityTypeBuilder<Advisory> builder)
        {
            builder.ToTable(nameof(Advisory));
            builder.HasKey(c => c.AdvisoryId);
            builder.Property(c => c.SectionName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.SchoolYear)
                .HasColumnType("varchar(150)");

            //Foreign Key
            builder.HasOne(c => c.ClassAdviserLink)
                .WithMany(c => c.AdvisoryList)
                .HasForeignKey(c => c.ClassAdviserId);

            builder.HasOne(c => c.StudentLink)
                .WithMany(c => c.AdvisoryList)
                .HasForeignKey(c => c.StudentLRN);
        }
    }








}
