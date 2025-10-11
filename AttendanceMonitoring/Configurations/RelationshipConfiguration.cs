using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceMonitoring.Configurations
{
    public class RelationshipConfiguration : IEntityTypeConfiguration<Relationship>
    {
        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            builder.ToTable(nameof(Relationship));
            builder.HasKey(c => c.RelationshipId);
            builder.Property(c => c.RelationshipType)
                .HasColumnType("varchar(150)");

            //Foreign Key
            builder.HasOne(c => c.StudentLink)
                .WithMany(c => c.RelationshipList)
                .HasForeignKey(c => c.StudentLRN);

            builder.HasOne(c => c.ParentLink)
                .WithMany(c => c.RelationshipList)
                .HasForeignKey(c => c.ParentId);


        }
    }








}
