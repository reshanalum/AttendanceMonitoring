using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class SpecificStudentListVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        private string _searchText;

        public ObservableCollection<StudentsinSection> Students { get; set; } = new ObservableCollection<StudentsinSection>();

        public ICommand ShowEditStudentCommand { get; set; }
        public ICommand ShowAddStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand ShowStudentInformationCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private StudentsinSection selectedStudent;
        public StudentsinSection SelectedStudent
        {
            get => selectedStudent;
            set
            {
                selectedStudent = value;
                OnPropertyChanged();
            }
        }
        //pagination
        public ObservableCollection<StudentsinSection> PagedStudents { get; set; } = new ObservableCollection<StudentsinSection>();
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
            TotalPages = (int)Math.Ceiling((double)Students.Count / ItemsPerPage);

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
            var items = Students
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            foreach (var item in items)
                PagedStudents.Add(item);

            OnPropertyChanged(nameof(PagedStudents));
        }

        public SpecificStudentListVM(DashboardVM dashboardVM, SectionDisplay section)
        {
            _dashboardVM = dashboardVM;
            selectedSection = section;

            ShowEditStudentCommand = new RelayCommand(ExecuteEditStudentCommand);
            ShowAddStudentCommand = new RelayCommand(ExecuteAddStudentCommand);
            DeleteStudentCommand = new RelayCommand(DeleteStudent);
            ShowStudentInformationCommand = new RelayCommand(ExecuteShowStudentInformation);

            LoadStudentsForSection();
            CurrentPage = 1;

            //Pagination
            NextPageCommand = new RelayCommand(_ => GoToPage(CurrentPage + 1), _ => CurrentPage < TotalPages);
            PrevPageCommand = new RelayCommand(_ => GoToPage(CurrentPage - 1));
            GoToPageCommand = new RelayCommand(page => GoToPage((int)page));

            UpdatePagination();

        }

        private void LoadStudentsForSection()
        {
            using var context = new AttendanceMonitoringContext();

            // Step 1: Fetch student data from DB
            var studentsFromDb = context.Advisories
                .Where(a => a.SectionName == selectedSection.SectionName)
                .Select(a => a.StudentLink)
                .ToList(); // Materialize query to memory

            // Step 2: Map to StudentsinSection with display numbers
            Students.Clear();
            int number = 1;
            foreach (var s in studentsFromDb)
            {
                Students.Add(new StudentsinSection
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    LRN = s.LRN,
                    EnrollmentStatus = s.EnrollmentStatus,
                    DisplayNumber = number++
                });
            }
        }


        private void RecalculateDisplayNumbers()
        {
            for (int i = 0; i < Students.Count; i++)
                Students[i].DisplayNumber = i + 1;
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

            Students.Remove(SelectedStudent);
            RecalculateDisplayNumbers(); // Update numbering dynamically
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

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                FilterStudents();
                OnPropertyChanged();
            }
        }

        private void FilterStudents()
        {
            LoadStudentsForSection();

            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            var search = SearchText.ToLower();
            var filtered = Students
                .Where(s => s.FirstName.ToLower().Contains(search) ||
                            s.LastName.ToLower().Contains(search) ||
                            s.LRN.ToLower().Contains(search))
                .ToList();

            Students.Clear();
            foreach (var s in filtered)
                Students.Add(s);

            RecalculateDisplayNumbers(); // Ensure dynamic numbering after filter
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

    public class StudentsinSection : NotifyPropertyChanged
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LRN { get; set; }
        public string EnrollmentStatus { get; set; }

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

        public string FullName => $"{FirstName} {LastName}";
    }


}
