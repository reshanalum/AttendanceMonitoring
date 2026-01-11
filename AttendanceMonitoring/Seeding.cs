using AttendanceMonitoring.Models;
using Bogus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoring
{
    internal class Seeding
    {
        [Test]
        public void SeedTest()
        {
            using (var context = new AttendanceMonitoringContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // 1️⃣ BASE ENTITIES -----------------------------------
                var students = new Faker<Student>()
                    .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                    .RuleFor(c => c.LastName, f => f.Name.LastName())
                    .RuleFor(c => c.LRN, f => f.Random.ReplaceNumbers("###########"))
                    .RuleFor(c => c.RFID, f => f.Random.ReplaceNumbers("?# ?# ?# ?#"))
                    .RuleFor(c => c.EnrollmentStatus, f => f.PickRandom("Enrolled", "Not Enrolled"))
                    .Generate(30);

                var parents = new Faker<Parent>()
                    .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                    .RuleFor(c => c.LastName, f => f.Name.LastName())
                    .Generate(30);

                var advisers = new Faker<Class_Adviser>()
                    .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                    .RuleFor(c => c.LastName, f => f.Name.LastName())
                    .Generate(10);

                context.Students.AddRange(students);
                context.Parents.AddRange(parents);
                context.Class_Advisers.AddRange(advisers);
                context.SaveChanges(); // IDs assigned here

                // 2️⃣ SECONDARY ENTITIES -------------------------------
                var contacts = new Faker<Contact>()
                    .RuleFor(c => c.ParentId, f => f.PickRandom(parents).ParentId)
                    .RuleFor(c => c.PhoneNumber, f => f.Random.ReplaceNumbers("09#########"))
                    .Generate(30);

                var advisories = new Faker<Advisory>()
                    .RuleFor(c => c.StudentId, f => f.PickRandom(students).StudentId)
                    .RuleFor(c => c.ClassAdviserId, f => f.PickRandom(advisers).ClassAdviserId)
                    .RuleFor(c => c.SectionName, f => f.Commerce.Department())
                    .RuleFor(c => c.SchoolYear, f => f.Date.Past(2).Year.ToString())
                    .Generate(30);

                var attendances = new Faker<Attendance>()
                    .RuleFor(c => c.StudentId, f => f.PickRandom(students).StudentId)
                    .RuleFor(c => c.DateTime, f => f.Date.Between(new DateTime(2025, 1, 1), DateTime.Now))
                    .RuleFor(c => c.Status, f => f.PickRandom("IN", "OUT"))
                    .Generate(50);

                context.Contacts.AddRange(contacts);
                context.Advisories.AddRange(advisories);
                context.Attendances.AddRange(attendances);
                context.SaveChanges(); // IDs assigned here

                // 3️⃣ TERTIARY ENTITIES -------------------------------
                //var notifications = new Faker<Notification>()
                //    .RuleFor(c => c.AttendanceId, f => f.PickRandom(attendances).AttendanceId)
                //    .RuleFor(c => c.Message, f => f.PickRandom("ABOT NA IMONG ANAK LODS", "PASI NA IMONG ANAK LODS"))
                //    .Generate(50);

                //context.Notifications.AddRange(notifications);
                context.SaveChanges(); // Needed before Delivered

                // 4️⃣ FINAL DEPENDENTS -------------------------------
                //var delivered = new Faker<Delivered>()
                //    .RuleFor(c => c.NotificationId, f => f.PickRandom(notifications).NotificationId)
                //    .RuleFor(c => c.ContactId, f => f.PickRandom(contacts).ContactId)
                //    .RuleFor(c => c.DateTimeSent, f => f.Date.Between(new DateTime(2025, 1, 1), DateTime.Now))
                //    .Generate(50);

                var relationships = new Faker<Relationship>()
                    .RuleFor(c => c.StudentId, f => f.PickRandom(students).StudentId)
                    .RuleFor(c => c.ParentId, f => f.PickRandom(parents).ParentId)
                    .RuleFor(c => c.RelationshipType, f => f.PickRandom("PARENT", "GUARDIAN"))
                    .Generate(30);

                //context.Delivereds.AddRange(delivered);
                context.Relationships.AddRange(relationships);
                context.SaveChanges();
            }
        }

    }
}
