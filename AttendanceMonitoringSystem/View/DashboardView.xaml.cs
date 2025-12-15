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
    public partial class DashboardView : Window
    {
        private DashboardVM _vm;
        public DashboardView()
        {
            InitializeComponent();
            _vm = new DashboardVM(); 
            DataContext = _vm;

            Window window = Window.GetWindow(this);
            if (window != null)
            {
                //window.WindowStyle = WindowStyle.None;  // Removes the title bar
                window.WindowState = WindowState.Maximized; // Makes it fullscreen
                window.ResizeMode = ResizeMode.NoResize; // Prevents resizing
            }
            else
            {
                Loaded += (s, e) =>
                {
                    var win = Window.GetWindow(this);
                    //win.WindowStyle = WindowStyle.None;
                    win.WindowState = WindowState.Maximized;
                    win.ResizeMode = ResizeMode.NoResize;
                };
            }
        }
        private void ShowLogout(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

                var loginWindow = new LoginView();
                loginWindow.Show();

                // Close the current window that’s showing the dashboard
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.Close();
            }
        }

        private void ShowDashboard(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowDashboard();
        }

        private void ShowSectionList(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowSectionList();
        }

        private void ShowStudentList(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowStudentPage();
        }

        private void ShowTeacherList(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowTeacherPage();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ShowAdminSettings(object sender, RoutedEventArgs e)
        {
            _vm.ExecuteShowAdminSettings();

        }
    }
}
