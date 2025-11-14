using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class StudentInformationVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private List<Attendance> _attendanceList;
        public List<Attendance> AttendanceList
        {
            get => _attendanceList;
            set
            {
                _attendanceList = value;
                OnPropertyChanged(nameof(AttendanceList));
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public Attendance SelectedAttendance { get; set; }

        public RelayCommand BackCommand { get; }

        // For ComboBox
        public List<string> EnrollmentStatus { get; } = new List<string> { "Enrolled", "Dropped", "Pending" };

        public StudentInformationVM(Student student, DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            SelectedStudent = student;

            LoadAttendanceHistory();

            BackCommand = new RelayCommand(ExecuteBackCommand);
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
    }
}
