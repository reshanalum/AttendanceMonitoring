using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Command;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using AttendanceMonitoringSystem.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class SectionListVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<SectionDisplay> AdvisoryList { get; set; } = new ObservableCollection<SectionDisplay>();
        
        private SectionDisplay _selectedSection;
        public SectionDisplay SelectedSection
        {
            get => _selectedSection;
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection = value;
                    OnPropertyChanged();
                    (ShowEditSectionCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        private int _selectedIndex;
        private string _sectionSearchText;
        public string SectionSearchText
        {
            get => _sectionSearchText;
            set
            {
                _sectionSearchText = value;
                FilterSections();
                OnPropertyChanged();
            }
        }

        public ICommand ShowEditSectionCommand { get; set; }
        public ICommand ShowAddSectionCommand { get; set; }
        public ICommand ViewSectionCommand { get; set; }

        public SectionListVM(DashboardVM dashboardVM)
        {
            LoadSections();
            ShowAddSectionCommand = new RelayCommand(ExecuteAddSectionCommand);
            ShowEditSectionCommand = new RelayCommand(ExecuteEditSectionCommand, CanExecuteEditSectionCommand);
            ViewSectionCommand = new RelayCommand(ExecuteViewSectionCommand);
            _dashboardVM = dashboardVM;
        }

        private bool CanExecuteEditSectionCommand(object obj)
        {
            return true;
        }

        private void ExecuteViewSectionCommand(object obj)
        {
            if (obj is SectionDisplay section)
            {
                var sectionDetailsView = new SectionDetailsView(_dashboardVM, section);
                _dashboardVM.CurrentView = sectionDetailsView;
            }
        }

        private void ExecuteEditSectionCommand(object obj)
        {
            if (SelectedSection == null)
            {
                MessageBox.Show("Please select a section before editing.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var advisoryToEdit = context.Advisories
                .Include(a => a.ClassAdviserLink)
                .FirstOrDefault(a => a.SectionName == SelectedSection.SectionName);

            if (advisoryToEdit != null)
            {
                var editView = new EditSectionView();
                editView.DataContext = new EditSectionVM(_dashboardVM, advisoryToEdit);
                _dashboardVM.CurrentView = editView;
            }
            else
            {
                MessageBox.Show("Selected section could not be found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteAddSectionCommand(object obj)
        {
            var addView = new AddSectionView();
            addView.DataContext = new AddSectionVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }

      

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; OnPropertyChanged(); }
        }

        private void FilterSections()
        {
            string search = SectionSearchText?.Trim().ToLower() ?? "";

            using var context = new AttendanceMonitoringContext();
            var sections = context.Advisories
                .GroupBy(a => new
                {
                    a.SectionName,
                    AdviserName = a.ClassAdviserLink.FirstName + " " + a.ClassAdviserLink.LastName
                })
                .Select(g => new SectionDisplay
                {
                    SectionName = g.Key.SectionName,
                    AdviserName = g.Key.AdviserName,
                    StudentCount = g.Count()
                })
                .Where(s =>
                    s.SectionName.ToLower().Contains(search) ||
                    s.AdviserName.ToLower().Contains(search))
                .ToList();

            AdvisoryList.Clear();
            foreach (var section in sections)
                AdvisoryList.Add(section);
        }

        private void LoadSections()
        {
            using var context = new AttendanceMonitoringContext();

            var sections = context.Advisories
                .GroupBy(a => new
                {
                    a.SectionName,
                    AdviserName = a.ClassAdviserLink.FirstName + " " + a.ClassAdviserLink.LastName
                })
                .Select(g => new SectionDisplay
                {
                    SectionName = g.Key.SectionName,
                    AdviserName = g.Key.AdviserName,
                    StudentCount = g.Count()
                })
                .ToList();

            AdvisoryList.Clear();
            foreach (var section in sections)
                AdvisoryList.Add(section);
        }
    }
}
