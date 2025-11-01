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
using System.Windows.Shapes;
using AttendanceMonitoring.ViewModel;

namespace TEVES_FP_WPF_PCSHOP.View
{
    /// <summary>
    /// Interaction logic for PCLoginView.xaml
    /// </summary>
    public partial class PCLoginView : Window
    {
        public PCLoginView()
        {
            InitializeComponent();
            DateContext = new LoginViewModel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e) 
        { 
            WindowState = WindowState.Minimized;
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e) 
        {
            Application.Current.Shutdown();
        }
    }
}
