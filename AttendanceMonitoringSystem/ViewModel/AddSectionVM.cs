using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class AddSectionVM
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<StudentDisplay> StudentList { get; set; } = new();
        public ObservableCollection<Class_Adviser> TeacherList { get; set; } = new ObservableCollection<Class_Adviser>();
        public string NewSectionName { get; set; }
        public string NewSelectedTeacher {  get; set; }
        public string SelectedStudent { get; set; }
        public int? SelectedTeacherId { get; set; }

        public AddSectionVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            LoadStudents();
            LoadTeachers();
        }

        private void LoadTeachers()
        {
            using var context = new AttendanceMonitoringContext();
            var teachers = context.Class_Advisers.Select(c => new Class_Adviser
            {
                ClassAdviserId = c.ClassAdviserId,
                FirstName = c.FirstName,
                LastName = c.LastName,

            }).ToList();

            TeacherList.Clear();
            foreach (var t in teachers)
            {
                TeacherList.Add(t);
            }
        }

        private void LoadStudents()
        {
            using var context = new AttendanceMonitoringContext();
            var students = context.Students
                .Select(c => new StudentDisplay
                {
                    Student = c,
                    IsSelected = false
                })
                .ToList();

            StudentList.Clear();
            foreach (var s in students)
            {
                StudentList.Add(s);
            }
        }
        public void SaveCommand()
        {
            if (string.IsNullOrWhiteSpace(NewSectionName) || SelectedTeacherId == null)
            {
                MessageBox.Show("Please enter a section name and select a teacher.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedStudents = StudentList.Where(s => s.IsSelected).ToList();

            if (!selectedStudents.Any())
            {
                MessageBox.Show("Please select at least one student.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            foreach (var s in selectedStudents)
            {
                var newAdvisory = new Advisory
                {
                    SectionName = NewSectionName,
                    SchoolYear = "2025-2026",
                    ClassAdviserId = SelectedTeacherId.Value, // converted int
                    StudentId = s.Student.StudentId
                };

                context.Advisories.Add(newAdvisory);
            }

            context.SaveChanges();

            MessageBox.Show("Section successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            BackToSectionList();
        }

        public void BackToSectionList()
        {
            var sectionView = new SectionListView(_dashboardVM);
            sectionView.DataContext = new SectionListVM(_dashboardVM);
            _dashboardVM.CurrentView = sectionView;
        }

    }
}
