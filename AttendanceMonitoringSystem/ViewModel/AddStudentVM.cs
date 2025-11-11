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


        public string NewStudentID { get; set; }


        public string NewFirstName { get; set; }
        public string NewLastName { get; set; }
        public string NewLRN { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewEnrollmentStatus { get; set; }

        public string NewParentID { get; set; }     
        public string NewParentFirstName { get; set; }
        public string NewParentLastName { get; set; }
        public string NewParentEmail { get; set; }

        public string NewParentContactNumber1 { get; set; }
        public string NewParentContactNetwork1 { get; set; }  //NEED PA BA TO? WALA SIYA NAKALAGAY SA UI

        public string NewParentContactNumber2 { get; set; }
        public string NewParentContactNetwork2 { get; set; }


        public AddStudentVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            GenerateUniqueStudentId();
            GenerateUniqueParentId();

        }


        //Methods
        private void GenerateUniqueStudentId() 
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            string randomStudentId;

            do { randomStudentId = random.Next(1000, 9999).ToString(); }
            while (context.Students.Any(c => c.StudentId == randomStudentId));

            NewStudentID = randomStudentId;
            OnPropertyChanged(nameof(NewStudentID));
        }

        private void GenerateUniqueParentId()
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            string randomParentId;

            do { randomParentId = random.Next(1000, 9999).ToString(); }
            while (context.Students.Any(c => c.StudentId == randomParentId));

            NewParentID = randomParentId;
            OnPropertyChanged(nameof(NewParentID));
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
                PhoneNumber = NewPhoneNumber,
                EnrollmentStatus = NewEnrollmentStatus
            };

            context.Students.Add(newStudent);
            context.SaveChanges();

            var newParent = new Parent
            {
                FirstName = NewParentFirstName,
                LastName = NewParentLastName,
                Email = NewParentEmail
            };

            context.Parents.Add(newParent);
            context.SaveChanges();

            // Save the first contact (mandatory)
            var contact1 = new Contact
            {
                ParentId = newParent.ParentId,
                PhoneNumber = NewParentContactNumber1,
                Network = NewParentContactNetwork1 ?? ""
            };
            context.Contacts.Add(contact1);

            // Save the second contact (optional)
            if (!string.IsNullOrWhiteSpace(NewParentContactNumber2))
            {
                var contact2 = new Contact
                {
                    ParentId = newParent.ParentId,
                    PhoneNumber = NewParentContactNumber2,
                    Network = NewParentContactNetwork2 ?? ""
                };
                context.Contacts.Add(contact2);
            }

            context.SaveChanges();

            var relationship = new Relationship
            {
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
