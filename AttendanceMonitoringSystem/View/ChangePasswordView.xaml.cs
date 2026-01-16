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
    /// Interaction logic for ChangePasswordView.xaml
    /// </summary>
    public partial class ChangePasswordView : UserControl
    {

        public ChangePasswordView(DashboardVM dashboardVM)
        {
            InitializeComponent();
            _vm = new ChangePasswordVM(dashboardVM);
            this.DataContext = _vm;
        }

        private ChangePasswordVM _vm;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChangePasswordVM vm)
                vm.Save();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Cancel();  
        }
    }
}
