using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoring.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(c => c.UserId);
            builder.Property(c => c.FirstName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.LastName)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.Email)
                .HasColumnType("varchar(150)");
            builder.Property(c => c.IsAdmin)
                .HasColumnType("bit");
        }
    }

}
