using AttendanceMonitoringSystem.Commands;
using GardenGloryApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace AttendanceMonitoringSystem.ViewModel
{
    public class DashboardVM: NotifyPropertyChanged
    {
        public ICommand ShowClassListCommand { get; set; }
        public ICommand ShowDashboardCommand { get; set; }

        private NotifyPropertyChanged _currentView;

        public NotifyPropertyChanged CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public DashboardVM()
        {
            ShowClassListCommand = new RelayCommand(ExecuteShowClassListCommand);
            ShowDashboardCommand = new RelayCommand(ExecuteShowDashboardCommand);
        }

        private void ExecuteShowClassListCommand(object obj)
        {
            CurrentView = new ClassListVM();

        }

        private void ExecuteShowDashboardCommand(object obj)
        {
            CurrentView = null;

        }



    }
}
