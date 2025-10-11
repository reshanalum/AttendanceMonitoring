using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification));
            builder.HasKey(c => c.NotificationId);
            builder.Property(c => c.Message)
                .HasColumnType("varchar(400)");

            //Foreign Key
            builder.HasOne(c => c.AttendanceLink)
                .WithMany(c => c.NotificationList)
                .HasForeignKey(c => c.AttendanceId);


        }
    }








}
