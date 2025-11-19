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
    public class StudentListVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        public ICommand ShowEditStudentCommand { get; set; }
        public ICommand ShowAddStudentCommand { get; set; }

        public ICommand DeleteStudentCommand { get; set; }

        public ICommand ShowStudentInformationCommand { get; set; }

        public string SelectedSectionName { get; set; }

        private List<Student> _allStudents;

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



        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();

        private Student selectedStudent;

        public Student SelectedStudent
        {
            get => selectedStudent;
            set
            {
                selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private void FilterStudents()
        {
            string search = StudentSearchText?.Trim().ToLower() ?? "";

            var filtered = _allStudents
                .Where(s =>
                    s.FirstName.ToLower().Contains(search) ||
                    s.LastName.ToLower().Contains(search) ||
                    s.StudentId.ToString().Contains(search) ||
                    s.LRN.ToLower().Contains(search) ||
                    s.EnrollmentStatus.ToLower().Contains(search)
                )
                .ToList();

            StudentList.Clear();
            foreach (var student in filtered)
                StudentList.Add(student);
        }



        private void LoadStudents()
        {

            using var context = new AttendanceMonitoringContext();

            _allStudents = context.Students.ToList();  // Master list

            StudentList.Clear();
            foreach (var student in _allStudents)
                StudentList.Add(student);

        }

        public StudentListVM(DashboardVM dashboardVM)
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
            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new StudentInformation();
            view.DataContext = new StudentInformationVM(SelectedStudent, _dashboardVM);
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
            StudentList.Remove(SelectedStudent);
            SelectedStudent = null;
        }

        private void ExecuteEditStudentCommand(object obj)
        {

            var student = SelectedStudent;

            if (student == null)
            {
                MessageBox.Show("Please select a student to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
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
}
