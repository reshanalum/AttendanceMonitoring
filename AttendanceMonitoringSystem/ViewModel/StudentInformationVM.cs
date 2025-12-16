using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class StudentInformationVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        
        public Student SelectedStudent { get; }
        public Parent Parent { get; }
        public Contact? Contact1 { get; }
        public Contact? Contact2 { get; }

        private List<Attendance> _attendanceList;
        public List<Attendance> AttendanceList
        {
            get => _attendanceList;
            private set
            {
                _attendanceList = value;
                OnPropertyChanged(nameof(AttendanceList));
            }
        }

        // Commands
        public RelayCommand BackCommand { get; }
        public RelayCommand EditStudentCommand { get; }

        public StudentInformationVM(Student student, DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            using var context = new AttendanceMonitoringContext();

            var studentInDb = context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (studentInDb == null) throw new System.Exception("Selected student not found.");
            SelectedStudent = studentInDb;

   
            var relationship = context.Relationships.FirstOrDefault(r => r.StudentId == SelectedStudent.StudentId);
            if (relationship == null) throw new System.Exception("Parent relationship not found.");

            var parentInDb = context.Parents.FirstOrDefault(p => p.ParentId == relationship.ParentId);
            if (parentInDb == null) throw new System.Exception("Parent not found in DB.");
            Parent = parentInDb;

            var contacts = context.Contacts
                .Where(c => c.ParentId == Parent.ParentId)
                .ToList();
            Contact1 = contacts.Count > 0 ? contacts[0] : null;
            Contact2 = contacts.Count > 1 ? contacts[1] : null;
            LoadAttendanceHistory();

   
            BackCommand = new RelayCommand(ExecuteBackCommand);
            EditStudentCommand = new RelayCommand(ExecuteEditStudent);
        }

        private void LoadAttendanceHistory()
        {
            using var context = new AttendanceMonitoringContext();
            AttendanceList = context.Attendances
                .Where(a => a.StudentId == SelectedStudent.StudentId)
                .OrderByDescending(a => a.DateTime)
                .ToList();
        }

        private void ExecuteBackCommand(object obj)
        {
            var view = new StudentListView(_dashboardVM);
            view.DataContext = new StudentListVM(_dashboardVM);
            _dashboardVM.CurrentView = view;
        }

        private void ExecuteEditStudent(object obj)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var student = context.Students.FirstOrDefault(s => s.StudentId == SelectedStudent.StudentId);
            if (student == null)
            {
                MessageBox.Show("Student not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editView = new EditStudent();
            editView.DataContext = new EditStudentVM(student, _dashboardVM);
            _dashboardVM.CurrentView = editView;
        }
    }
}
