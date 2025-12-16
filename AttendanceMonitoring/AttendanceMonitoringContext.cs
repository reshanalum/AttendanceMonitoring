using AttendanceMonitoring.Configurations;
using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoring
{
    public class AttendanceMonitoringContext: DbContext
    {
        public DbSet<Advisory> Advisories { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class_Adviser> Class_Advisers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Delivered> Delivereds { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=db\AttendanceMonitoringFINALPROMISE.db");
            //optionsBuilder.UseSqlite("Data Source=C:\\Users\\Hi\\Desktop\\AttendanceMonitoring1\\AttendanceMonitoringFINALPROMISE.db");
            //optionsBuilder.UseSqlite("Data Source=C:\\Users\\MYPC\\Documents\\Fergie Codes\\Final_SLP_AttendanceMonitoringSystemCode\\AttendanceMonitoringFINALPROMISE.db"); //fergie comment this before pushing to github
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\15-FB1019AX r5\\OneDrive\\Documents\\Fergie Codes\\AttendanceMonitoring\\AttendanceMonitoring-master\\AttendanceMonitoringFINALPROMISE.db");


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdvisoryConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
            modelBuilder.ApplyConfiguration(new ClassAdviserConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveredConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new ParentConfiguration());
            modelBuilder.ApplyConfiguration(new RelationshipConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

        }



    }
}
