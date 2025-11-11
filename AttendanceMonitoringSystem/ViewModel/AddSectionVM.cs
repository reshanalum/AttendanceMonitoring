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
            throw new NotImplementedException();
        }

        private void LoadStudents()
        {
            throw new NotImplementedException();
        }
    }
}
