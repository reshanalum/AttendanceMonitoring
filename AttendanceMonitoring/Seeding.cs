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
            //using (var context = new AttendanceMonitoringContext())
            //{
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();

            //    var enrollmentStatus = new List<string>()
            //    {
            //        "Enrolled", "Not Enrolled"
            //    };
            //    var studentGen = new Faker<Student>()
            //        .RuleFor(c => c.StudentId, f => f.Random.Replace("CA-##****"))
            //        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            //        .RuleFor(c => c.LastName, f => f.Name.LastName())
            //        .RuleFor(c => c.LRN, f => f.Random.ReplaceNumbers("###########"))
            //        .RuleFor(c => c.PhoneNumber, f => f.Random.ReplaceNumbers("09#########"))
            //        .RuleFor(c => c.EnrollmentStatus, f => f.PickRandom(enrollmentStatus));
            //    var students = studentGen.Generate(50);

            //    var parentGen = new Faker<Parent>()
            //        .RuleFor(c => c.ParentId, f => f.Random.Replace("PR-##****"))
            //        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            //        .RuleFor(c => c.LastName, f => f.Name.LastName())
            //        .RuleFor(c => c.Email, f => f.Internet.Email());
            //    var parents = parentGen.Generate(50);

            //    var networks = new List<string>()
            //    {
            //        "TNT", "SMART", "DITO", "GLOBE", "SUN"
            //    };

            //    var contactGen = new Faker<Contact>()
            //        .RuleFor(c => c.ContactId, f => f.Random.Replace("CON-##**"))
            //        .RuleFor(c => c.ParentId, f => f.PickRandom(parents).ParentId)
            //        .RuleFor(c => c.PhoneNumber, f => f.Random.ReplaceNumbers("09#########"))
            //        .RuleFor(c => c.Network, f => f.PickRandom(networks));
            //    var contacts = contactGen.Generate(50);

            //    var adviserGen = new Faker<Class_Adviser>()
            //        .RuleFor(c => c.ClassAdviserId, f => f.Random.Replace("CA-##****"))
            //        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            //        .RuleFor(c => c.LastName, f => f.Name.LastName())
            //        .RuleFor(c => c.PhoneNumber, f => f.Random.ReplaceNumbers("09#########"));
            //    var advisers = adviserGen.Generate(50);

            //    var advisoryGen = new Faker<Advisory>()
            //        .RuleFor(c => c.AdvisoryId, f => f.Random.Replace("AD-??##?#"))
            //        .RuleFor(c => c.ClassAdviserId, f => f.PickRandom(advisers).ClassAdviserId)
            //        .RuleFor(c => c.StudentId, f => f.PickRandom(students).StudentId)
            //        .RuleFor(c => c.SectionName, f => f.Random.Word())
            //        .RuleFor(c => c.SchoolYear, f => f.Date.Past(2).Year.ToString());
            //    var advisories = advisoryGen.Generate(50);

            //    var attendanceStatus = new List<string>()
            //    {
            //        "IN", "OUT"
            //    };

            //    var attendanceGen = new Faker<Attendance>()
            //        .RuleFor(c => c.AttendanceId, f => f.Random.Replace("??##**"))
            //        .RuleFor(c => c.StudentId, f => f.PickRandom(students).StudentId)
            //        .RuleFor(c => c.DateTime, f => f.Date.Between(new DateTime(2025, 1, 1), DateTime.Now))
            //        .RuleFor(c => c.Status, f => f.PickRandom(attendanceStatus));
            //    var attendances = attendanceGen.Generate(50);

            //    var messages = new List<string>()
            //    {
            //        "ABOT NA IMONG ANAK LODS", "PASI NA IMONG ANAK LODS"
            //    };

            //    var notificationGen = new Faker<Notification>()
            //        .RuleFor(c => c.NotificationId, f => f.Random.Replace("NOTIF-*****"))
            //        .RuleFor(c => c.AttendanceId, f => f.PickRandom(attendances).AttendanceId)
            //        .RuleFor(c => c.Message, f => f.PickRandom(messages));
            //    var notifications = notificationGen.Generate(50);

            //    var deliveredGen = new Faker<Delivered>()
            //        .RuleFor(c => c.DeliveredId, f => f.Random.Replace("DEL-??##?#"))
            //        .RuleFor(c => c.NotificationId, f => f.PickRandom(notifications).NotificationId)
            //        .RuleFor(c => c.ContactId, f => f.PickRandom(contacts).ContactId)
            //        .RuleFor(c => c.DateTimeSent, f => f.Date.Between(new DateTime(2025, 1, 1), DateTime.Now));
            //    var delivers = deliveredGen.Generate(50);

            //    var relationshipType = new List<string>()
            //    {
            //        "PARENT", "GUARDIAN"
            //    };

            //    var relationshipGen = new Faker<Relationship>()
            //        .RuleFor(c => c.RelationshipId, f => f.Random.Replace("RS-*****"))
            //        .RuleFor(c => c.StudentId, f => f.PickRandom(students).StudentId)
            //        .RuleFor(c => c.ParentId, f => f.PickRandom(parents).ParentId)
            //        .RuleFor(c => c.RelationshipType, f => f.PickRandom(relationshipType));
            //    var relationships = relationshipGen.Generate(50);

            //    context.Set<Student>().AddRange(students);
            //    context.Set<Parent>().AddRange(parents);
            //    context.Set<Class_Adviser>().AddRange(advisers);
            //    context.Set<Advisory>().AddRange(advisories);
            //    context.Set<Attendance>().AddRange(attendances);
            //    context.Set<Notification>().AddRange(notifications);
            //    context.Set<Delivered>().AddRange(delivers);
            //    context.Set<Contact>().AddRange(contacts);
            //    context.Set<Relationship>().AddRange(relationships);
            //    context.SaveChanges();










            //}
        }
    }
}
