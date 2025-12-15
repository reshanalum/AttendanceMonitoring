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
    /// Interaction logic for ChangeUsernameView.xaml
    /// </summary>
    public partial class ChangeUsernameView : UserControl
    {
        public ChangeUsernameView()
        {
            InitializeComponent();
        }

        public ChangeUsernameView(DashboardVM dashboardVM)
        {
            InitializeComponent();
            _vm = new ChangeUsernameVM(dashboardVM);
            this.DataContext = _vm;
        }

        private ChangeUsernameVM _vm;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChangeUsernameVM vm)
                vm.Save();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Cancel();
        }

    }
}
