using AttendanceMonitoringSystem.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class PageButton : NotifyPropertyChanged
    {
        public int Number { get; set; }

        private int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(IsCurrent));
            }
        }

        public bool IsCurrent => Number == CurrentPage;
    }
}
