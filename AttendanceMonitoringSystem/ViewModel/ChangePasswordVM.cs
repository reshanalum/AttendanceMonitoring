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
    public class ChangePasswordVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        public ChangePasswordVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
        }

        private string _currentPassword;
        public string CurrentPassword
        {
            get => _currentPassword;
            set { _currentPassword = value; OnPropertyChanged(); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            using var context = new AttendanceMonitoringContext();

            var user = context.Users.FirstOrDefault(u => u.LastName == CurrentPassword);

            if (user == null)
            {
                MessageBox.Show("Current password is incorrect.");
                return;
            }

            user.LastName = NewPassword; 
            context.SaveChanges();

            MessageBox.Show("Password successfully updated.");

         
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
