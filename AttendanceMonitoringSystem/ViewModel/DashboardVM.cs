using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            var dashboardHomeView = new DashboardUserControlView(this);
            CurrentView = dashboardHomeView;
        }

        public void ExecuteShowStudentPage()
        {


            var studentListView = new StudentListView(this);
            CurrentView = studentListView;
        }
        
        public void ExecuteShowTeacherPage()
        {

            var teacherListView = new TeacherListView(this);
            CurrentView = teacherListView;

        }

        public void ExecuteShowNotificationList()
        {
            var notificationListView = new NotificationListView(this);
            CurrentView = notificationListView;

        }

        public void ExecuteShowAdminSettings()
        {
            var result = MessageBox.Show(
                "Would you like to change your username or password?",
                "Admin Settings",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CurrentView = new AdminSettingsView(this);
            }
            else
            {

                CurrentView = new DashboardUserControlView(this);
            }
        }


    }
}
