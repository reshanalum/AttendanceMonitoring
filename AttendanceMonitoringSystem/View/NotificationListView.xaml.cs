using AttendanceMonitoringSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttendanceMonitoringSystem.View
{
    /// <summary>
    /// Interaction logic for NotificationListView.xaml
    /// </summary>
    public partial class NotificationListView : UserControl
    {
        private NotificationListVM _vm;

        public NotificationListView(DashboardVM dashboardVM)
        {
            InitializeComponent();
            _vm = new NotificationListVM(dashboardVM);
            this.DataContext = _vm;

        }
    }
}
