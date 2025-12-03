using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class StudentListVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        public ICommand ShowEditStudentCommand { get; set; }
        public ICommand ShowAddStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand ShowStudentInformationCommand { get; set; }

        private List<StudentInDisplay> _allStudents;

        private string _studentSearchText;
        public string StudentSearchText
        {
            get => _studentSearchText;
            set
            {
                _studentSearchText = value;
                FilterStudents();
                OnPropertyChanged();
            }
        }

        //for pagination

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


        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();

        private Student selectedStudent;

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        public StudentListVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            ShowEditStudentCommand = new RelayCommand(ExecuteEditStudentCommand);
            ShowAddStudentCommand = new RelayCommand(ExecuteAddStudentCommand);
            DeleteStudentCommand = new RelayCommand(DeleteStudent);
            ShowStudentInformationCommand = new RelayCommand(ExecuteShowStudentInformation);

            LoadStudents();
        }

        private void LoadStudents()
        {
            using var context = new AttendanceMonitoringContext();

            _allStudents = context.Students
                .Select(s => new StudentInDisplay
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    LRN = s.LRN,
                    EnrollmentStatus = s.EnrollmentStatus
                })
                .ToList();

            RecalculateDisplayNumbers(_allStudents);
            RefreshStudentList(_allStudents);
        }

        private void RefreshStudentList(List<StudentInDisplay> students)
        {
            LoadStudents();
            _dashboardVM = dashboardVM;

            ShowEditStudentCommand = new RelayCommand(ExecuteEditStudentCommand);
            ShowAddStudentCommand = new RelayCommand(ExecuteAddStudentCommand);

            DeleteStudentCommand = new RelayCommand(DeleteStudent);
            ShowStudentInformationCommand = new RelayCommand(ExecuteShowStudentInformation);
        }

        private void ExecuteShowStudentInformation(object obj)
        {
            for (int i = 0; i < students.Count; i++)
                students[i].DisplayNumber = i + 1;
        }

        private void FilterStudents()
        {
            var search = StudentSearchText?.Trim().ToLower() ?? "";

            var filtered = string.IsNullOrWhiteSpace(search)
                ? _allStudents
                : _allStudents.Where(s =>
                    s.FirstName.ToLower().Contains(search) ||
                    s.LastName.ToLower().Contains(search) ||
                    s.LRN.ToLower().Contains(search) ||
                    s.EnrollmentStatus.ToLower().Contains(search) ||
                    s.StudentId.ToString().Contains(search))
                    .ToList();

            RecalculateDisplayNumbers(filtered);
            RefreshStudentList(filtered);
        }

        public void DeleteStudent(object obj)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("No student selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete student: {SelectedStudent.FirstName} {SelectedStudent.LastName}?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var context = new AttendanceMonitoringContext();
            var studentInDb = context.Students.FirstOrDefault(c => c.StudentId == SelectedStudent.StudentId);

            if (studentInDb == null)
            {
                MessageBox.Show("The selected student does not exist in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Students.Remove(studentInDb);
            context.SaveChanges();

            // Remove from master list and refresh
            _allStudents.Remove(SelectedStudent);
            RecalculateDisplayNumbers(_allStudents);
            RefreshStudentList(_allStudents);

            SelectedStudent = null;
        }

        private void ExecuteShowStudentInformation(object obj)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            // Fetch the real Student entity from the DB
            var student = context.Students
                .FirstOrDefault(s => s.StudentId == SelectedStudent.StudentId);

            if (student == null)
            {
                MessageBox.Show("Student not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var view = new StudentInformation();
            view.DataContext = new StudentInformationVM(student, _dashboardVM);
            _dashboardVM.CurrentView = view;
        }


        private void ExecuteEditStudentCommand(object obj)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var student = context.Students
                .FirstOrDefault(s => s.StudentId == SelectedStudent.StudentId);

            if (student == null)
            {
                MessageBox.Show("Student not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editView = new EditStudent();
            editView.DataContext = new EditStudentVM(student, _dashboardVM);
            _dashboardVM.CurrentView = editView;
        }


        private void ExecuteAddStudentCommand(object obj)
        {
            var addView = new AddStudentView();
            addView.DataContext = new AddStudentVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }
    }

    public class StudentInDisplay : NotifyPropertyChanged
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LRN { get; set; }
        public string EnrollmentStatus { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        private int _displayNumber;
        public int DisplayNumber
        {
            get => _displayNumber;
            set
            {
                if (_displayNumber != value)
                {
                    _displayNumber = value;
                    OnPropertyChanged();
                }
            }
        }
    }
    
}
