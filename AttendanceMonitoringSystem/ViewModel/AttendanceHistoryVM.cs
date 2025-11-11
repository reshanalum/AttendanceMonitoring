using AttendanceMonitoring;
using AttendanceMonitoring.Models;
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

        private Attendance _selectedAttendance;
        private int _selectedIndex;
        private string attendanceSearchText;

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
                    StudentId = c.StudentLink.FirstName,
                    DateTime = c.DateTime,
                    Status = c.Status,
                })
                .ToList();
        }

        public AttendanceHistoryVM(DashboardVM dashboardVM)
        {
            LoadAttendanceHistory();
            _dashboardVM = dashboardVM;
        }

        private void LoadAttendanceHistory()
        {
            using var context = new AttendanceMonitoringContext();
            var attendances = context.Attendances.Select(c => new Attendance
            {
                AttendanceId = c.AttendanceId,
                StudentId = c.StudentLink.FirstName,
                DateTime = c.DateTime,
                Status = c.Status,
            }).ToList();

            AttendanceList.Clear();
            foreach (var attendance in attendances)
            {
                AttendanceList.Add(attendance);
            }
        }
    }
}
