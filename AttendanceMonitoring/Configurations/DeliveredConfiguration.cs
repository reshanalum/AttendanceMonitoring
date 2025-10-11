using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class DeliveredConfiguration : IEntityTypeConfiguration<Delivered>
    {
        public void Configure(EntityTypeBuilder<Delivered> builder)
        {
            builder.ToTable(nameof(Delivered));
            builder.HasKey(c => c.DeliveredId);
            builder.Property(c => c.DateTimeSent)
                .HasColumnType("date");

            //Foreign Key
            builder.HasOne(c => c.NotificationLink)
                .WithMany(c => c.DeliveredList)
                .HasForeignKey(c => c.NotificationId);

            builder.HasOne(c => c.ContactLink)
                .WithMany(c => c.DeliveredList)
                .HasForeignKey(c => c.ContactId);

        }
    }








}
