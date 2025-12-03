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
    /// Interaction logic for EditTeacherView.xaml
    /// </summary>
    public partial class EditTeacherView : UserControl
    {
        private EditTeacherVM _vm;

        public EditTeacherView()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = (EditTeacherVM)DataContext;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.SaveCommand();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.BackToTeacherList();
            }
        }
    }
}
