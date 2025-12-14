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

        public ObservableCollection<StudentInDisplay> StudentList { get; set; }
            = new ObservableCollection<StudentInDisplay>();

        private StudentInDisplay _selectedStudent;
        public StudentInDisplay SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private List<StudentInDisplay> _allStudents;

        //pagination
        public ObservableCollection<StudentInDisplay> PagedStudents { get; set; } = new ObservableCollection<StudentInDisplay>();
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

        public int ItemsPerPage { get; set; } = 15;
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
        public StudentListVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            ShowEditStudentCommand = new RelayCommand(ExecuteEditStudentCommand);
            ShowAddStudentCommand = new RelayCommand(ExecuteAddStudentCommand);
            DeleteStudentCommand = new RelayCommand(DeleteStudent);
            ShowStudentInformationCommand = new RelayCommand(ExecuteShowStudentInformation);

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
            using var context = new AttendanceMonitoringContext();

            var students = context.Students
                .OrderBy(s => s.StudentId)
                .ToList();

  
            _allStudents = students
                .Select((s, index) => new StudentInDisplay
                {
                    StudentId = s.StudentId,
                    LRN = s.LRN,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    EnrollmentStatus = s.EnrollmentStatus,
                    DisplayNumber = index + 1
                })
                .ToList();

            StudentList.Clear();
            foreach (var student in _allStudents)
                StudentList.Add(student);
        }

        private void FilterStudents()
        {
            LoadStudents();

            if (string.IsNullOrWhiteSpace(StudentSearchText))
                return;

            var search = StudentSearchText.Trim().ToLower();

            var filtered = _allStudents
                .Where(s =>
                    (s.FirstName ?? "").ToLower().Contains(search) ||
                    (s.LastName ?? "").ToLower().Contains(search) ||
                    (s.LRN ?? "").ToLower().Contains(search) ||
                    (s.EnrollmentStatus ?? "").ToLower().Contains(search) ||
                    s.StudentId.ToString().Contains(search))
                .ToList();


            StudentList.Clear();
            foreach (var student in filtered)
                StudentList.Add(student);

            RecalculateDisplayNumbers();

            CurrentPage = 1;
            UpdatePagination();
        }

        private void RecalculateDisplayNumbers()
        {
            for (int i = 0; i < StudentList.Count; i++)
                StudentList[i].DisplayNumber = i + 1;
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

            StudentList.Remove(SelectedStudent);
            RecalculateDisplayNumbers();
            SelectedStudent = null;
        }

        private void ExecuteEditStudentCommand(object obj)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var student = context.Students.FirstOrDefault(s => s.StudentId == SelectedStudent.StudentId);

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

        private void ExecuteShowStudentInformation(object obj)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var student = context.Students.FirstOrDefault(s => s.StudentId == SelectedStudent.StudentId);

            if (student == null)
            {
                MessageBox.Show("Student not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var view = new StudentInformation();
            view.DataContext = new StudentInformationVM(student, _dashboardVM);
            _dashboardVM.CurrentView = view;
        }
    }

    public class StudentInDisplay : NotifyPropertyChanged
    {
        public int StudentId { get; set; }
        public string LRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
