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
    public class AttendanceHistoryVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        public ObservableCollection<Attendance> AttendanceList { get; set; } = new ObservableCollection<Attendance>();
        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();

        private Attendance _selectedAttendance;
        private int _selectedIndex;
        private string attendanceSearchText;
        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                LoadAttendanceForStudent(); 
            }
        }
        private void LoadAttendanceForStudent()
        {
            if (SelectedStudent == null)
            {
                AttendanceList.Clear();
                return;
            }

            using var context = new AttendanceMonitoringContext();

            var attendances = context.Attendances
                .Where(a => a.StudentId == SelectedStudent.StudentId)
                .Select(a => new Attendance
                {
                    AttendanceId = a.AttendanceId,
                    StudentId = a.StudentId,
                    DateTime = a.DateTime,
                    Status = a.Status,
                    StudentLink = a.StudentLink,
                })
                .ToList();

            AttendanceList.Clear();
            foreach (var attendance in attendances)
            {
                AttendanceList.Add(attendance);
            }
        }

        public Attendance SelectedAttendance
        {
            get { return _selectedAttendance; }
            set
            {
                _selectedAttendance = value;
            }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        public string AttendanceSearchText
        {
            get { return attendanceSearchText; }
            set
            {
                attendanceSearchText = value;
                FilterAttendances();
            }
        }


        public ObservableCollection<Student> PagedStudents { get; set; } = new ObservableCollection<Student>();
        private int currentPage { get; set; }
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged();
                LoadPage();
                UpdatePageButtons();
            }
        }

        private void UpdatePageButtons()
        {
            PageButtons.Clear();

            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, CurrentPage + 2);

            for (int i = start; i <= end; i++)
            {
                PageButtons.Add(new PageButton
                {
                    Number = i,
                    CurrentPage = CurrentPage
                });
            }

            OnPropertyChanged(nameof(PageButtons));
        }

        public int ItemsPerPage { get; set; } = 20;
        public int TotalPages { get; set; }
        public ObservableCollection<PageButton> PageButtons { get; set; } = new();
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand GoToPageCommand { get; }

        private void GoToPage(int v)
        {
            if (v < 1 || v > TotalPages)
            {
                return;
            }
            CurrentPage = v;
            LoadPage();
        }

        private void UpdatePagination()
        {
            TotalPages = (int)Math.Ceiling((double)StudentList.Count / ItemsPerPage);

            UpdatePageButtons();
            LoadPage();

            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(PageButtons));
        }

        private void LoadPage()
        {
            if (CurrentPage < 1) CurrentPage = 1;

            PagedStudents.Clear();
            var items = StudentList
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            foreach (var item in items)
                PagedStudents.Add(item);

            OnPropertyChanged(nameof(PagedStudents));
        }

        private void FilterAttendances()
        {
            string searchText = AttendanceSearchText?.ToLower() ?? string.Empty;
            using var context = new AttendanceMonitoringContext();
            var attendances = context.Attendances
                .Where(c => c.StudentLink.FirstName.ToLower().Contains(searchText) ||
                            c.StudentLink.LastName.ToLower().Contains(searchText) ||
                            c.Status.ToLower().Contains(searchText) ||
                            c.DateTime.ToString().ToLower().Contains(searchText))
                .Select(c => new Attendance
                {
                    AttendanceId = c.AttendanceId,
                    StudentId = c.StudentLink.StudentId, 
                    DateTime = c.DateTime,
                    Status = c.Status,
                })
                .ToList();
        }

        public AttendanceHistoryVM(DashboardVM dashboardVM, SectionDisplay section)
        {
            _dashboardVM = dashboardVM;
            selectedSection = section;
            LoadStudents();
            CurrentPage = 1;

            //Pagination
            NextPageCommand = new RelayCommand(_ => GoToPage(CurrentPage + 1), _ => CurrentPage < TotalPages);
            PrevPageCommand = new RelayCommand(_ => GoToPage(CurrentPage - 1));
            GoToPageCommand = new RelayCommand(page => GoToPage((int)page));

            UpdatePagination();

        }
        private void LoadStudents()
        {
            if (selectedSection == null) return;

            using var context = new AttendanceMonitoringContext();


            var studentsInSection = context.Advisories
                .Where(a => a.SectionName == selectedSection.SectionName)
                .Select(a => a.StudentLink)
                .ToList();


            var studentIdsInAdv = context.Advisories
                .Select(a => a.StudentId)
                .ToList();

            var noSectionStudents = context.Students
                .Where(s => !studentIdsInAdv.Contains(s.StudentId))
                .ToList();

   
            var allStudents = studentsInSection
                .Union(noSectionStudents)
                .Select(s => new Student
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    LRN = s.LRN,
                    EnrollmentStatus = s.EnrollmentStatus
                })
                .ToList();

   
            StudentList.Clear();
            foreach (var s in allStudents)
                StudentList.Add(s);
        }


        private SectionDisplay selectedSection; 
        public void SetSelectedSection(SectionDisplay section)
        {
            selectedSection = section;
        }

        public void BackToSectionDetailsList()
        {

            var sectionView = new SectionDetailsView(_dashboardVM, selectedSection);
            sectionView.DataContext = new SectionDetailsVM(_dashboardVM, selectedSection);
            _dashboardVM.CurrentView = sectionView;
        }
    }
}
