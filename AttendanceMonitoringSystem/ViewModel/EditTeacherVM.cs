using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Linq;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class EditTeacherVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        public Class_Adviser EditingTeacher { get; set; }

        public EditTeacherVM(Class_Adviser selectedTeacher, DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            LoadTeacher(selectedTeacher);
        }

        private void LoadTeacher(Class_Adviser teacher)
        {
            using var context = new AttendanceMonitoringContext();

            EditingTeacher = context.Class_Advisers
                .FirstOrDefault(t => t.ClassAdviserId == teacher.ClassAdviserId);

            if (EditingTeacher == null)
            {
                MessageBox.Show("Selected teacher not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                BackToTeacherList();
                return;
            }

            OnPropertyChanged(nameof(EditingTeacher));
        }

        public void SaveCommand()
        {
            if (EditingTeacher == null)
                return;

            using var context = new AttendanceMonitoringContext();

            var teacherInDb = context.Class_Advisers
                .FirstOrDefault(t => t.ClassAdviserId == EditingTeacher.ClassAdviserId);

            if (teacherInDb != null)
            {
                teacherInDb.FirstName = EditingTeacher.FirstName;
                teacherInDb.LastName = EditingTeacher.LastName;
            }

            context.SaveChanges();

            MessageBox.Show("Teacher information saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
