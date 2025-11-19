using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class DashboardUserControlVM : NotifyPropertyChanged
    {
        public ObservableCollection<SectionDisplay> PopulationData { get; set; } = new ObservableCollection<SectionDisplay>();
        public class AttendanceChartItem
        {
            public string DateLabel { get; set; }
            public int OnTime { get; set; }
            public int Late { get; set; }
        }
        public class SearchResult
        {
            public string DisplayName { get; set; } 
            public string Type { get; set; }        
            public object Reference { get; set; }   
        }

        public ObservableCollection<AttendanceChartItem> AttendanceDataChartList { get; set; }
            = new ObservableCollection<AttendanceChartItem>();

        private readonly DashboardVM _dashboardVM;
        public DashboardUserControlVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            LoadPieChartData();
            LoadAttendanceChartData();
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();  // Notify binding
                    FilterSearchItems();         // Update the filtered list
                }
            }
        }

        private void FilterSearchItems()
        {
            FilteredSearchItems.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var item in AllSearchItems)
                    FilteredSearchItems.Add(item);
                return;
            }

            var lower = SearchText.ToLower();
            var filtered = AllSearchItems
                .Where(x => x.DisplayName.ToLower().Contains(lower))
                .ToList();

            foreach (var item in filtered)
                FilteredSearchItems.Add(item);
        }

        public ObservableCollection<SearchResult> AllSearchItems { get; set; } = new ObservableCollection<SearchResult>();
        public ObservableCollection<SearchResult> FilteredSearchItems { get; set; } = new ObservableCollection<SearchResult>();
        private void LoadSearchItems()
        {
            using (var db = new AttendanceMonitoringContext())
            {
                // Students
                foreach (var s in db.Students.ToList())
                {
                    AllSearchItems.Add(new SearchResult
                    {
                        DisplayName = $"{s.FirstName} {s.LastName}",
                        Type = "Student",
                        Reference = s
                    });
                }

                // Sections (Advisories)
                foreach (var sec in db.Advisories.Select(a => a.SectionName).Distinct().ToList())
                {
                    AllSearchItems.Add(new SearchResult
                    {
                        DisplayName = sec,
                        Type = "Section",
                        Reference = sec
                    });
                }

                // Teachers (ClassAdvisers)
                foreach (var t in db.Class_Advisers.ToList())
                {
                    AllSearchItems.Add(new SearchResult
                    {
                        DisplayName = $"{t.FirstName} {t.LastName}",
                        Type = "Teacher",
                        Reference = t
                    });
                }
            }

        }

        private void LoadAttendanceChartData()
        {
            using (var db = new AttendanceMonitoringContext())
            {
                TimeSpan cutoff = new TimeSpan(7, 50, 0); // 7:50 AM late na

                var chartData = db.Attendances
                    .Where(a => a.Status == "IN") // Only consider arrivals
                    .AsEnumerable()
                    .GroupBy(a => a.DateTime.Date)
                    .Select(g => new AttendanceChartItem
                    {
                        DateLabel = g.Key.ToString("MMM dd"),     // e.g., "Nov 19"
                        OnTime = g.Count(a => a.DateTime.TimeOfDay <= cutoff),
                        Late = g.Count(a => a.DateTime.TimeOfDay > cutoff)
                    })
                    .OrderBy(x => x.DateLabel)
                    .ToList();

                AttendanceDataChartList.Clear();
                foreach (var item in chartData)
                    AttendanceDataChartList.Add(item);
            }
        }

        private void LoadPieChartData()
        {
            using (var db = new AttendanceMonitoringContext())
            {
                PopulationData.Clear();

                PopulationData.Add(new SectionDisplay
                {
                    SectionName = "Students",
                    StudentCount = db.Students.Count(),
                    //SliceColor = new SolidBrush(Color.FromArgb(0, 50, 112))
                });

                PopulationData.Add(new SectionDisplay
                {
                    SectionName = "Sections",
                    StudentCount = db.Advisories
                                    .Select(a => a.SectionName)
                                    .Distinct()
                                    .Count(),
                    //SliceColor = new SolidBrush(Color.FromArgb(0, 92, 207))
                });

                PopulationData.Add(new SectionDisplay
                {
                    SectionName = "Class Advisers",
                    StudentCount = db.Class_Advisers.Count(),
                    //SliceColor = new SolidBrush(Color.FromArgb(79, 157, 255))
                });
            }
        }
    }
}
