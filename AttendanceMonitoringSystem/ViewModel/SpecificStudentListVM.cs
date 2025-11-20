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
    class SpecificStudentListVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        private string _sectionName;
        private string _searchText;

        public ObservableCollection<StudentsinSection> Students { get; set; }
            = new ObservableCollection<StudentsinSection>();

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

        public SpecificStudentListVM(DashboardVM dashboardVM, string sectionName)
        {
            _dashboardVM = dashboardVM;
            _sectionName = sectionName;

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ShowEditStudentCommand = new RelayCommand(ExecuteEditStudentCommand);
            ShowAddStudentCommand = new RelayCommand(ExecuteAddStudentCommand);

            DeleteStudentCommand = new RelayCommand(DeleteStudent);
            ShowStudentInformationCommand = new RelayCommand(ExecuteShowStudentInformation);

            LoadStudentsForSection();
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

        private void LoadStudentsForSection()
        {
            using var context = new AttendanceMonitoringContext();

            var students = context.Advisories
                .Where(a => a.SectionName == _sectionName)
                .Select(a => a.StudentLink)
                .Select(s => new StudentsinSection   
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    LRN = s.LRN,
                    EnrollmentStatus = s.EnrollmentStatus
                })
                .ToList();

            Students.Clear();
            foreach (var s in students)
                Students.Add(s);
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
                .Where(s =>
                    s.FirstName.ToLower().Contains(search) ||
                    s.LastName.ToLower().Contains(search) ||
                    s.LRN.ToLower().Contains(search))
                .ToList();

            Students.Clear();
            foreach (var s in filtered)
                Students.Add(s);
        }


        private void ExecuteBackCommand(object obj)
        {
            var sectionDetails = new SectionDetailsView(_dashboardVM,
                new SectionDisplay { SectionName = _sectionName });

            sectionDetails.DataContext = new SectionDetailsVM(_dashboardVM,
                new SectionDisplay { SectionName = _sectionName });

            _dashboardVM.CurrentView = sectionDetails;
        }
    }

    public class StudentsinSection
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LRN { get; set; }
        public string EnrollmentStatus { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}

