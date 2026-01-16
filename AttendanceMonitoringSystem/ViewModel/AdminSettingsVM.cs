using AttendanceMonitoring;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class AdminSettingsVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        private object _newView;
        public object NewView
        {
            get { return _newView; }
            set
            {
                _newView = value;
                OnPropertyChanged(nameof(NewView));
            }
        }

        public AdminSettingsVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
        }

        public void ExecuteChangePass()
        {
            var view = new ChangePasswordView(_dashboardVM);
            view.DataContext = new ChangePasswordVM(_dashboardVM);
            _dashboardVM.CurrentView = view;
        }

        public void ExecuteChangeUser()
        {
            var view = new ChangeUsernameView(_dashboardVM);
            view.DataContext = new ChangeUsernameVM(_dashboardVM);
            _dashboardVM.CurrentView = view;
        }

        public void ExecuteCancel()
        {
            var view = new DashboardUserControlView(_dashboardVM);
            view.DataContext = new DashboardUserControlVM(_dashboardVM);
            _dashboardVM.CurrentView = view;
        }


    }

}
