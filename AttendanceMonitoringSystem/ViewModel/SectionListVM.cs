using AttendanceMonitoringSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Command;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class SectionListVM : NotifyPropertyChanged
    {
        public ObservableCollection<Advisory> AdvisoryList { get; set; } = new ObservableCollection<Advisory>();
        
        private Advisory _selectedSection;
        private int _selectedIndex;

        public Advisory SelectedSection
        {
            get{ return _selectedSection; }
            set{ _selectedSection = value;
                OnPropertyChanged("SelectedSection");
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        public SectionListVM()
        {
           
        }
    }
}
