using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
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
    public class SectionDisplay
    {
        public string SectionName { get; set; }
        public string AdviserName { get; set; }
        public int StudentCount { get; set; }

    }
}
