using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class AttendanceHistoryVM
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<Attendance> AttendanceList { get; set; } = new ObservableCollection<Attendance>();
        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();

        private Attendance _selectedAttendance;
        private int _selectedIndex;
        private string attendanceSearchText;
        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                LoadAttendanceForStudent(); // call whenever user selects a student
            }
        }
        private void LoadAttendanceForStudent()
        {
            if (SelectedStudent == null)
            {
                AttendanceList.Clear();
                return;
            }

            using var context = new AttendanceMonitoringContext();

            var attendances = context.Attendances
                .Where(a => a.StudentId == SelectedStudent.StudentId)
                .Select(a => new Attendance
                {
                    AttendanceId = a.AttendanceId,
                    StudentId = a.StudentId,
                    DateTime = a.DateTime,
                    Status = a.Status,
                    StudentLink = a.StudentLink,
                })
                .ToList();

            AttendanceList.Clear();
            foreach (var attendance in attendances)
            {
                AttendanceList.Add(attendance);
            }
        }

        public Attendance SelectedAttendance
        {
            get { return _selectedAttendance; }
            set
            {
                _selectedAttendance = value;
            }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        public string AttendanceSearchText
        {
            get { return attendanceSearchText; }
            set
            {
                attendanceSearchText = value;
                FilterAttendances();
            }
        }

        private void FilterAttendances()
        {
            string searchText = AttendanceSearchText?.ToLower() ?? string.Empty;
            using var context = new AttendanceMonitoringContext();
            var attendances = context.Attendances
                .Where(c => c.StudentLink.FirstName.ToLower().Contains(searchText) ||
                            c.StudentLink.LastName.ToLower().Contains(searchText) ||
                            c.Status.ToLower().Contains(searchText) ||
                            c.DateTime.ToString().ToLower().Contains(searchText))
                .Select(c => new Attendance
                {
                    AttendanceId = c.AttendanceId,
                    StudentId = c.StudentLink.StudentId, //FIRST NAME OF THE STUDENT
                    DateTime = c.DateTime,
                    Status = c.Status,
                })
                .ToList();
        }

        public AttendanceHistoryVM(DashboardVM dashboardVM)
        {
            //LoadAttendanceHistory();
            LoadAttendanceForStudent();
            LoadStudents();
            _dashboardVM = dashboardVM;
        }
        private void LoadStudents()
        {
            using var context = new AttendanceMonitoringContext();

            var students = context.Students.Select(c => new Student
            {
                StudentId = c.StudentId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                LRN = c.LRN,
                EnrollmentStatus = c.EnrollmentStatus
            }).ToList();

            StudentList.Clear();

            foreach (var s in students)
            {
                StudentList.Add(s);
            }
        }
        private SectionDisplay selectedSection; // add this in your class

        public void SetSelectedSection(SectionDisplay section)
        {
            selectedSection = section;
        }

        public void BackToSectionDetailsList()
        {

            var sectionView = new SectionDetailsView(_dashboardVM, selectedSection);
            sectionView.DataContext = new SectionDetailsVM(_dashboardVM, selectedSection);
            _dashboardVM.CurrentView = sectionView;
        }
    }
}
