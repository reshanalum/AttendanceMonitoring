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
    /// Interaction logic for AddStudentView.xaml
    /// </summary>
    public partial class AddStudentView : UserControl
    {
        public AddStudentView()
        {
            InitializeComponent();
            AddStudentVM addstudentVM = new AddStudentVM();
            this.DataContext = addstudentVM;
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {

        }
        private void SaveButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
