using AttendanceMonitoringSystem.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class NewUserVM: NotifyPropertyChanged
    {

        private readonly DashboardVM _dashboardVM;
        public NewUserVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
        }
    }
}
