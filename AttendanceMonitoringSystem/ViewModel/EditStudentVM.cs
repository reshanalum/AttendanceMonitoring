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
    public class EditStudentVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        private Student _editingStudent;

        public Student EditingStudent
        {
            get => _editingStudent;
            set
            {
                _editingStudent = value;
                OnPropertyChanged();
            }
        }

        public EditStudentVM(Student student, DashboardVM dashboardVM)
        {

            EditingStudent = student;
            _dashboardVM = dashboardVM;
        }

        public void SaveEditedEmployee()
        {
            if (EditingStudent == null) return;

            using var context = new AttendanceMonitoringContext();

            var studentInDb = context.Students
                .FirstOrDefault(c => c.StudentId == EditingStudent.StudentId);

            if (studentInDb != null)
            {
                studentInDb.FirstName = EditingStudent.FirstName;
                studentInDb.LastName = EditingStudent.LastName;
                studentInDb.PhoneNumber = EditingStudent.PhoneNumber;
                studentInDb.EnrollmentStatus = EditingStudent.EnrollmentStatus;

                context.SaveChanges();
                MessageBox.Show("Student details updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Student not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

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
