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
using AttendanceMonitoringSystem.ViewModel;

namespace AttendanceMonitoringSystem.View
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        private DashboardVM _vm;
        public DashboardView()
        {
            InitializeComponent();
            _vm = new DashboardVM(); 
            DataContext = _vm;
        }

        private void Logout(object sender, RoutedEventArgs e)
        {


        }
        private void ShowDashboard(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowDashboard();
        }

        private void ShowSectionList(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowSectionList();
        }

        private void ShowTeacherList(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowTeacherPage();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
