using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class AddTeacherVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        // Class Adviser properties
        public int NewClassAdviserId { get; set; }
        public string NewFirstName { get; set; }
        public string NewLastName { get; set; }


        public AddTeacherVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            GenerateUniqueClassAdviserId();
        }

        private void GenerateUniqueClassAdviserId()
        {
            using var context = new AttendanceMonitoringContext();
            var random = new Random();
            int id;
            do
            {
                id = random.Next(1000, 9999);
            } while (context.Class_Advisers.Any(c => c.ClassAdviserId == id));

            NewClassAdviserId = id;
            OnPropertyChanged(nameof(NewClassAdviserId));
        }

        public void SaveCommand()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName) ||
                string.IsNullOrWhiteSpace(NewLastName))
            {
                MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            // Add Class Adviser
            var classAdviser = new Class_Adviser
            {
                ClassAdviserId = NewClassAdviserId,
                FirstName = NewFirstName,
                LastName = NewLastName,
                AdvisoryList = new List<Advisory>()
            };
            context.Class_Advisers.Add(classAdviser);

            context.SaveChanges();

            MessageBox.Show($"New Class Adviser {NewFirstName} {NewLastName} added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            BackToTeacherList();
        }

        public void BackToTeacherList()
        {
            var teacherListView = new TeacherListView(_dashboardVM);
            teacherListView.DataContext = new TeacherListVM(_dashboardVM);
            _dashboardVM.CurrentView = teacherListView;
        }
    }
}
