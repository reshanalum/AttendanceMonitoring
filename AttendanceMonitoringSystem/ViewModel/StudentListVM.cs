using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AttendanceMonitoringSystem.Commands;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class StudentListVM: NotifyPropertyChanged
    {

        public ICommand ShowEditStudentCommand { get; set; }
        public ICommand ShowAddStudentCommand { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public StudentListVM()
        {

        }

        public void ExecuteShowSectionList()
        {

            var sectionListView = new SectionListView();
            CurrentView = sectionListView;
        }

        public void ExecuteShowDashboard()
        {

            //var employeesView = new Employees(this, Permissions);
            //CurrentView = employeesView;
            //Caption = "Employee";
        }


    }
}
