using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable(nameof(Contact));
            builder.HasKey(c => c.ContactId);
            builder.Property(c => c.PhoneNumber)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.Network)
                .HasColumnType("varchar(150)");

            //Foreign Key
            builder.HasOne(c => c.ParentLink)
                .WithMany(c => c.ContactList)
                .HasForeignKey(c => c.ParentId);

        }
    }








}
