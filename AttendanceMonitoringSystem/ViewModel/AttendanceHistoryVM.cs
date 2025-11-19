using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public AttendanceHistoryVM(DashboardVM dashboardVM, SectionDisplay section)
        {
            _dashboardVM = dashboardVM;
            selectedSection = section;
            LoadStudents();

        }
        private void LoadStudents()
        {
            if (selectedSection == null) return;

            using var context = new AttendanceMonitoringContext();

            // Students in this section
            var studentsInSection = context.Advisories
                .Where(a => a.SectionName == selectedSection.SectionName)
                .Select(a => a.StudentLink)
                .ToList();

            // Students NOT in any section
            var studentIdsInAdv = context.Advisories
                .Select(a => a.StudentId)
                .ToList();

            var noSectionStudents = context.Students
                .Where(s => !studentIdsInAdv.Contains(s.StudentId))
                .ToList();

            // Combine both lists
            var allStudents = studentsInSection
                .Union(noSectionStudents)
                .Select(s => new Student
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    LRN = s.LRN,
                    EnrollmentStatus = s.EnrollmentStatus
                })
                .ToList();

            // Update observable list
            StudentList.Clear();
            foreach (var s in allStudents)
                StudentList.Add(s);
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
