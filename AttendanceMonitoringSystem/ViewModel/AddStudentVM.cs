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


        public AddStudentVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            GenerateUniqueStudentId();
        }


        //Methods
        private void GenerateUniqueStudentId() 
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            string randomId;

            do { randomId = random.Next(1000, 9999).ToString(); }
            while (context.Students.Any(c => c.StudentId == randomId));

            NewStudentID = randomId;
            OnPropertyChanged(nameof(NewStudentID));
        }


        public void SaveCommand()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName)
                || string.IsNullOrWhiteSpace(NewLastName)
                || string.IsNullOrWhiteSpace(NewLRN)
                || string.IsNullOrWhiteSpace(NewPhoneNumber)
                || string.IsNullOrWhiteSpace(NewEnrollmentStatus))
            {
                MessageBox.Show("A requirement is missing.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            MessageBox.Show($"New student added with ID: {NewStudentID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
