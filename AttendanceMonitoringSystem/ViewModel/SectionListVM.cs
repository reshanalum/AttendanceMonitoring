using AttendanceMonitoringSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Command;
using AttendanceMonitoring;
using System.Windows.Input;
using AttendanceMonitoringSystem.View;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class SectionListVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<SectionDisplay> AdvisoryList { get; set; } = new ObservableCollection<SectionDisplay>();
        
        private SectionDisplay _selectedSection;
        private int _selectedIndex;
        private string studentSearchText;

        public ICommand ShowEditSectionCommand { get; set; }
        public ICommand ShowAddSectionCommand { get; set; }

        public SectionListVM(DashboardVM dashboardVM)
        {
            LoadSections();
            ShowAddSectionCommand = new RelayCommand(ExecuteAddSectionCommand);
            ShowEditSectionCommand = new RelayCommand(ExecuteEditSectionCommand);
            _dashboardVM = dashboardVM;
        }

        private void ExecuteEditSectionCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteAddSectionCommand(object obj)
        {
            var addView = new AddSectionView();
            addView.DataContext = new AddSectionVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }

        public SectionDisplay SelectedSection
        {
            get => _selectedSection; 
            set{ _selectedSection = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; OnPropertyChanged(); }
        }

        public string StudentSearchText
        {
            get { return studentSearchText; }
            set 
            { 
                studentSearchText = value;
                FilterSections();
                OnPropertyChanged();
            }
        }

        private void FilterSections()
        {
            string search = StudentSearchText?.ToLower() ?? "";

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
