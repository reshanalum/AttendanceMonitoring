using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace AttendanceMonitoringSystem.ViewModel
{
    public class DashboardVM: NotifyPropertyChanged
    {
        public ICommand ShowSectionListCommand { get; set; }
        public ICommand ShowDashboardCommand { get; set; }

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

        public DashboardVM()
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
