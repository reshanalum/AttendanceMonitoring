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
    /// Interaction logic for AdminSettingsView.xaml
    /// </summary>
    public partial class AdminSettingsView : UserControl
    {
        public AdminSettingsView(DashboardVM dashboardVM)
        {
            InitializeComponent();
            _vm = new AdminSettingsVM(dashboardVM);
            this.DataContext = _vm;
        }

        private AdminSettingsVM _vm;

        private void ChangePassword_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.ExecuteChangePass();
        }

        private void ChangeUsername_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.ExecuteChangeUser();
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.ExecuteCancel();
        }

    }
}
