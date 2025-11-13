using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class SectionDetailsVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        public SectionDisplay SelectedSection { get; }

        public ICommand ShowStudentListCommand { get; set; }
        public ICommand ShowAttendanceHistoryCommand { get; set; }

        private SectionDisplay _section;

        public SectionDisplay Section
        {
            get => _section;
            set
            {
                _section = value;
                OnPropertyChanged();
            }
        }



        public SectionDetailsVM(DashboardVM dashboardVM, SectionDisplay selectedSection)
        {
            ShowStudentListCommand = new RelayCommand(ExecuteShowStudentListCommand);
            ShowAttendanceHistoryCommand = new RelayCommand(ExecuteAttendanceHistoryCommand);

            _dashboardVM = dashboardVM;
            SelectedSection = selectedSection;
        }

        private void ExecuteShowStudentListCommand(object obj)
        {
            var addView = new StudentListView(_dashboardVM);
            addView.DataContext = new StudentListVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }

        private void ExecuteAttendanceHistoryCommand(object obj)
        {
            var addView = new AttendanceHistoryView(_dashboardVM);
            addView.DataContext = new AttendanceHistoryVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }

        public void BackToSectionList()
        {
            var sectionView = new SectionListView(_dashboardVM);
            sectionView.DataContext = new SectionListVM(_dashboardVM);
            _dashboardVM.CurrentView = sectionView;
        }
    }
}
