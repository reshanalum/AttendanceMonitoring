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
    /// Interaction logic for DashboardUserControlView.xaml
    /// </summary>
    public partial class DashboardUserControlView : UserControl
    {
        private DashboardUserControlVM _vm;
        public DashboardUserControlView(DashboardVM dashboardVM)
        {
            InitializeComponent();
            _vm = new DashboardUserControlVM(dashboardVM);
            this.DataContext = _vm;
        }

        internal void Show()
        {
            throw new NotImplementedException();
        }
    }
}
