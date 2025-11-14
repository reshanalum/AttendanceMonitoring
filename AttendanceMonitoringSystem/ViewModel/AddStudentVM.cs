using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class AddStudentVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;


        public int NewStudentID { get; set; }


        public string NewFirstName { get; set; }
        public string NewLastName { get; set; }
        public string NewLRN { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewEnrollmentStatus { get; set; }

        public int NewParentID { get; set; }     
        public string NewParentFirstName { get; set; }
        public string NewParentLastName { get; set; }
        public string NewParentEmail { get; set; }

        public int NewContact1ID { get; set; }

        public string NewParentContactNumber1 { get; set; }
        public string NewParentContactNetwork1 { get; set; }  //NEED PA BA TO? WALA SIYA NAKALAGAY SA UI

        public int NewContact2ID { get; set; }

        public string NewParentContactNumber2 { get; set; }
        public string NewParentContactNetwork2 { get; set; }

        public int NewRelationshipID { get; set; }

        public string SectionName { get; set; }

        public List<string> EnrollmentStatus { get; set; }



        public AddStudentVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            //SectionName = sectionName;

            EnrollmentStatus = new List<string>
                                {
                                    "Enrolled",
                                    "Not Enrolled",
                                };

            GenerateUniqueStudentId();
            GenerateUniqueParentId();
            GenerateUniqueContactId();
            GenerateRelationshipId();
        }



        //Methods
        private void GenerateUniqueStudentId() 
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            int randomStudentId;

            do { randomStudentId = random.Next(1000, 9999); }
            while (context.Students.Any(c => c.StudentId == randomStudentId));

            NewStudentID = randomStudentId;
            OnPropertyChanged(nameof(NewStudentID));
        }

        private void GenerateRelationshipId()
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            int randomRelationshipId;

            do { randomRelationshipId = random.Next(1000, 9999); }
            while (context.Relationships.Any(c => c.RelationshipId == randomRelationshipId));

            NewRelationshipID = randomRelationshipId;
            OnPropertyChanged(nameof(NewRelationshipID));
        }

        private void GenerateUniqueParentId()
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            int randomParentId;

            do { randomParentId = random.Next(1000, 9999); }
            while (context.Parents.Any(c => c.ParentId == randomParentId));

            NewParentID = randomParentId;
            OnPropertyChanged(nameof(NewParentID));
        }
        private void GenerateUniqueContactId()
        {
            using var context = new AttendanceMonitoringContext();
            var random1 = new Random();
            int randomContact1Id;

            do { randomContact1Id = random1.Next(1000, 9999); }
            while (context.Contacts.Any(c => c.ContactId == randomContact1Id));

            NewContact1ID = randomContact1Id;
            OnPropertyChanged(nameof(NewContact1ID));

            var random2 = new Random();
            int randomContact2Id;

            do { randomContact2Id = random2.Next(1000, 9999); }
            while (context.Contacts.Any(c => c.ContactId == randomContact2Id));

            NewContact2ID = randomContact2Id;
            OnPropertyChanged(nameof(NewContact2ID));
        }


        public void SaveCommand()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName)
                || string.IsNullOrWhiteSpace(NewLastName)
                || string.IsNullOrWhiteSpace(NewLRN)
                || string.IsNullOrWhiteSpace(NewPhoneNumber)
                || string.IsNullOrWhiteSpace(NewEnrollmentStatus)
                || string.IsNullOrWhiteSpace(NewParentFirstName)     
                || string.IsNullOrWhiteSpace(NewParentLastName)
                || string.IsNullOrWhiteSpace(NewParentEmail))
            {
                MessageBox.Show("A requirement is missing.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewParentContactNumber1))
            {
                MessageBox.Show("At least one parent contact number is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            var newStudent = new Student
            {
                StudentId = NewStudentID,
                FirstName = NewFirstName,
                LastName = NewLastName,
                LRN = NewLRN,
                EnrollmentStatus = NewEnrollmentStatus
            };

            context.Students.Add(newStudent);
            context.SaveChanges();

            var newParent = new Parent
            {
                ParentId = NewParentID,
                FirstName = NewParentFirstName,
                LastName = NewParentLastName
            };

            context.Parents.Add(newParent);
            context.SaveChanges();

            // Save the first contact (mandatory)
            var contact1 = new Contact
            {
                ContactId = NewContact1ID,
                ParentId = newParent.ParentId,
                PhoneNumber = NewParentContactNumber1
            };
            context.Contacts.Add(contact1);

            // Save the second contact (optional)
            if (!string.IsNullOrWhiteSpace(NewParentContactNumber2))
            {
                var contact2 = new Contact
                {
                    ContactId = NewContact2ID,
                    ParentId = newParent.ParentId,
                    PhoneNumber = NewParentContactNumber2
                };
                context.Contacts.Add(contact2);
            }

            context.SaveChanges();

            var relationship = new Relationship
            {
                RelationshipId = NewRelationshipID,
                StudentId = newStudent.StudentId,
                ParentId = newParent.ParentId,
                RelationshipType = "Parent"
            };

            context.Relationships.Add(relationship);
            context.SaveChanges();

            MessageBox.Show($"New student added with ID: {newStudent.StudentId}, parent and contacts saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            BackToStudentList();
        }

        public void BackToStudentList()
        {
            var studentView = new StudentListView(_dashboardVM);
            studentView.DataContext = new StudentListVM(_dashboardVM);
            _dashboardVM.CurrentView = studentView;
        }
    }
}
