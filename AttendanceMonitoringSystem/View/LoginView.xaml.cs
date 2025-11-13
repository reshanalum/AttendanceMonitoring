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
using System.Windows.Shapes;

namespace AttendanceMonitoringSystem.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel _vm;
        public LoginView()
        {
            InitializeComponent();
            _vm = new LoginViewModel();
            DataContext = _vm;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnADD_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;

            if (username == "1" && password == "1") // example validation
            {
                // Open Dashboard Window
                DashboardView dashboard = new DashboardView();
                dashboard.Show();

                // Close Login Window
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
