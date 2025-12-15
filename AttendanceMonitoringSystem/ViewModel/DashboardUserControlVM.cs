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
using LiveCharts;
using LiveCharts.Wpf;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class DashboardUserControlVM : NotifyPropertyChanged
    {
        public ObservableCollection<SectionDisplay> PopulationData { get; set; } = new ObservableCollection<SectionDisplay>();
        public SeriesCollection PopulationSeries { get; set; }
        public SeriesCollection AttendanceSeries { get; set; }
        public string[] AttendanceLabels { get; set; }

        public class AttendanceChartItem
        {
            public string DateLabel { get; set; }
            public int OnTime { get; set; }
            public int Late { get; set; }
        }
        public ObservableCollection<AttendanceChartItem> AttendanceDataChartList { get; set; }
            = new ObservableCollection<AttendanceChartItem>();

        private readonly DashboardVM _dashboardVM;
        public DashboardUserControlVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;
            LoadPieChartData();

            BuildPopulationChart();
            BuildAttendanceChart();
        }
        private void BuildPopulationChart()
        {
            PopulationSeries = new SeriesCollection();

            foreach (var item in PopulationData)
            {
                PopulationSeries.Add(new PieSeries
                {
                    Title = item.SectionName,
                    Values = new ChartValues<int> { item.StudentCount },
                    DataLabels = true
                });
            }

            OnPropertyChanged(nameof(PopulationSeries));
        }
        private void BuildAttendanceChart()
        {
            AttendanceSeries = new SeriesCollection
    {
        new LineSeries
        {
            Title = "On Time",
            Values = new ChartValues<int>(
                AttendanceDataChartList.Select(x => x.OnTime)
            ),
            PointGeometrySize = 10
        },
        new LineSeries
        {
            Title = "Late",
            Values = new ChartValues<int>(
                AttendanceDataChartList.Select(x => x.Late)
            ),
            PointGeometrySize = 10
        }
    };

            AttendanceLabels = AttendanceDataChartList
                .Select(x => x.DateLabel)
                .ToArray();

            OnPropertyChanged(nameof(AttendanceSeries));
            OnPropertyChanged(nameof(AttendanceLabels));
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

                });

                PopulationData.Add(new SectionDisplay
                {
                    SectionName = "Sections",
                    StudentCount = db.Advisories
                                    .Select(a => a.SectionName)
                                    .Distinct()
                                    .Count(),

                });

                PopulationData.Add(new SectionDisplay
                {
                    SectionName = "Class Advisers",
                    StudentCount = db.Class_Advisers.Count(),

                });
            }
        }
    }
}
