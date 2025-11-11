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

            var sectionListView = new SectionListView(this);
            CurrentView = sectionListView;
        }

        public void ExecuteShowDashboard()
        {

            CurrentView = null;
        }

        public void ExecuteShowTeacherPage()
        {
            //temporary lang

            var studentListView = new StudentListView(this);
            CurrentView = studentListView;
        }




    }
}
