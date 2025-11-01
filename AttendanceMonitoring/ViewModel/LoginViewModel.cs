using AttendanceMonitoring.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AttendanceMonitoring.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _username = "";
        private string _password = "";

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommands(Login, CanLogin);
        }

        private bool CanLogin(object parameter)
        {
            return true;
        }

        private void Login(object parameter)
        {
           
        }
    }
}
