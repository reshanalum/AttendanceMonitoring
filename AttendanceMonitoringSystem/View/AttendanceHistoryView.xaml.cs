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
    /// Interaction logic for AttendanceHistoryView.xaml
    /// </summary>
    public partial class AttendanceHistoryView : UserControl
    {
        private AttendanceHistoryVM _vm;
        public AttendanceHistoryView(DashboardVM dashboardVM, SectionDisplay section)
        {
            InitializeComponent();
            _vm = new AttendanceHistoryVM(dashboardVM, section);
            this.DataContext = _vm;
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.BackToSectionDetailsList(); // Navigate back
            }
        }
    }
}
