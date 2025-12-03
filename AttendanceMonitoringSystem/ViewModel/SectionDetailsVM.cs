using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            var studentListView = new SpecificStudentListView(_dashboardVM, SelectedSection);
            studentListView.DataContext = new SpecificStudentListVM(_dashboardVM, SelectedSection);
            _dashboardVM.CurrentView = studentListView;
        }

        private void ExecuteAttendanceHistoryCommand(object obj)
        {
            if (SelectedSection == null)
            {
                MessageBox.Show("Please select a section first.", "No Section Selected",
                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new AttendanceHistoryView(_dashboardVM, SelectedSection);
            _dashboardVM.CurrentView = view;

        }

        public void BackToSectionList()
        {
            var sectionView = new SectionListView(_dashboardVM);
            sectionView.DataContext = new SectionListVM(_dashboardVM);
            _dashboardVM.CurrentView = sectionView;
        }
    }
}
