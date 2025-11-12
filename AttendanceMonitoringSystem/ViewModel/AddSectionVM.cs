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
    public class AddSectionVM
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();
        public ObservableCollection<Class_Adviser> TeacherList { get; set; } = new ObservableCollection<Class_Adviser>();
        public string NewSectionName { get; set; }

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
                PhoneNumber = c.PhoneNumber

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

            var students = context.Students.Select(c => new Student
            {
                StudentId = c.StudentId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                LRN = c.LRN,
                PhoneNumber = c.PhoneNumber,
                EnrollmentStatus = c.EnrollmentStatus
            }).ToList();

            StudentList.Clear();

            foreach (var s in students)
            {
                StudentList.Add(s);
            }
        }
    }
}
