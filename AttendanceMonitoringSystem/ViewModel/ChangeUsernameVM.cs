using AttendanceMonitoring;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class ChangeUsernameVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        public ChangeUsernameVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
        }

        private string _currentUsername;
        public string CurrentUsername
        {
            get => _currentUsername;
            set { _currentUsername = value; OnPropertyChanged(); }
        }

        private string _newUsername;
        public string NewUsername
        {
            get => _newUsername;
            set { _newUsername = value; OnPropertyChanged(); }
        }

        private string _confirmUsername;
        public string ConfirmUsername
        {
            get => _confirmUsername;
            set { _confirmUsername = value; OnPropertyChanged(); }
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentUsername) ||
                string.IsNullOrWhiteSpace(NewUsername) ||
                string.IsNullOrWhiteSpace(ConfirmUsername))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (NewUsername != ConfirmUsername)
            {
                MessageBox.Show("New usernames do not match.");
                return;
            }

            using var context = new AttendanceMonitoringContext();

            var user = context.Users
                .FirstOrDefault(u => u.FirstName == CurrentUsername);

            if (user == null)
            {
                MessageBox.Show("Current username is incorrect.");
                return;
            }

            user.FirstName = NewUsername;
            context.SaveChanges();

            MessageBox.Show("Username successfully updated.");

            _dashboardVM.CurrentView = new AdminSettingsView(_dashboardVM)
            {
                DataContext = new AdminSettingsVM(_dashboardVM)
            };
        }

        public void Cancel()
        {
            _dashboardVM.CurrentView = new AdminSettingsView(_dashboardVM)
            {
                DataContext = new AdminSettingsVM(_dashboardVM)
            };
        }
    }
}
