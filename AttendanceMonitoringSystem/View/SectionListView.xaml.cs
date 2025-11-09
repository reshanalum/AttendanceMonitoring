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
    /// Interaction logic for SectionListView.xaml
    /// </summary>
    public partial class SectionListView : UserControl
    {
        public SectionListView()
        {
            InitializeComponent();
            SectionListVM sectionlistVM = new SectionListVM();
            this.DataContext = sectionlistVM;
        }

        private void SectionListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddSection(object sender, RoutedEventArgs e)
        {

        }

        private void EditSection(object sender, RoutedEventArgs e)
        {

        }

    }


}
