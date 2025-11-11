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

namespace AttendanceMonitoringSystem.ViewModel
{
    public class SectionListVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<Advisory> AdvisoryList { get; set; } = new ObservableCollection<Advisory>();
        
        private Advisory _selectedSection;
        private int _selectedIndex;
        private string studentSearchText;

        public SectionListVM(DashboardVM dashboardVM)
        {
            LoadSections();
            _dashboardVM = dashboardVM;
        }
        public Advisory SelectedSection
        {
            get => _selectedSection; 
            set{ _selectedSection = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        public string StudentSearchText
        {
            get { return studentSearchText; }
            set 
            { 
                studentSearchText = value;
                FilterStudents();
                OnPropertyChanged();
            }
        }

        private void FilterStudents()
        {
            string search = StudentSearchText.ToLower();

            using var context = new AttendanceMonitoringContext();
            var sections = context.Advisories
                .GroupBy(c => new
                {
                    c.AdvisoryId,
                    c.SectionName,
                    c.SchoolYear,
                    AdviserName = c.ClassAdviserLink.FirstName
                })
                .Select(g => new Advisory
                {
                    AdvisoryId = g.Key.AdvisoryId,
                    SectionName = g.Key.SectionName,
                    SchoolYear = g.Key.SchoolYear,
                    ClassAdviserId = g.Key.AdviserName,
                    StudentId = g.Count().ToString()
                })
                .Where(c =>
                    c.SectionName.ToLower().Contains(search) ||
                    c.SchoolYear.ToLower().Contains(search) ||
                    c.ClassAdviserId.ToLower().Contains(search) ||
                    c.StudentId.ToLower().Contains(search))
                .ToList();

            AdvisoryList.Clear();
            foreach (var section in sections)
            {
                AdvisoryList.Add(section);
            }
        }

        private void LoadSections()
        {
            using var context = new AttendanceMonitoringContext();

            var sections = context.Advisories
                .GroupBy(c => new
                {
                    c.AdvisoryId,
                    c.SectionName,
                    c.SchoolYear,
                    AdviserName = c.ClassAdviserLink.FirstName
                })
                .Select(g => new Advisory
                {
                    AdvisoryId = g.Key.AdvisoryId,
                    SectionName = g.Key.SectionName,
                    SchoolYear = g.Key.SchoolYear,
                    ClassAdviserId = g.Key.AdviserName,
                    StudentId = g.Count().ToString()
                })
                .ToList();

            AdvisoryList.Clear();
            foreach (var section in sections)
            {
                AdvisoryList.Add(section);
            }
        }
    }
}
