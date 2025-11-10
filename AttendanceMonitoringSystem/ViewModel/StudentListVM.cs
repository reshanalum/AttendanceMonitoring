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

        private string studentSearchText;

        public string StudentSearchText
        {
            get => studentSearchText;
            set
            {
                studentSearchText = value;
                FilterStudents();
            }
        }

        private void LoadStudents()
        {
            using var context = new AttendanceMonitoringContext();

            var students = context.Students.Select(c => new Student
            {
                StudentId = c.StudentId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                LRN = c.LRN,
                PhoneNumber = c.PhoneNumber,
                EnrollmentStatus = c.EnrollmentStatus
            }).ToList();

            StudentList.Clear();

            foreach (var s in students)
            {
                StudentList.Add(s);
            }
        }

        public StudentListVM(DashboardVM dashboardVM)
        {
            LoadStudents();
            ShowEditStudentCommand = new RelayCommand(ExecuteEditStudentCommand);
            ShowAddStudentCommand = new RelayCommand(ExecuteAddStudentCommand);
            _dashboardVM = dashboardVM;

        }

        public void DeleteStudent(object obj)
        {

            if (obj is not Student studentToDelete)
            {
                MessageBox.Show("No student selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete student: {studentToDelete.FirstName} {studentToDelete.LastName}?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var context = new AttendanceMonitoringContext();
            var studentInDb = context.Students.FirstOrDefault(c => c.StudentId == studentToDelete.StudentId);

            if (studentInDb == null)
            {
                MessageBox.Show("The selected student does not exist in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Students.Remove(studentInDb);
            context.SaveChanges();
            StudentList.Remove(studentToDelete);
            SelectedStudent = null;
        }

        private void ExecuteEditStudentCommand(object obj)
        {
            if (obj is Student student)
            {
                SelectedStudent = student;
                var editView = new EditStudent();
                editView.DataContext = new EditStudentVM(student,_dashboardVM);
                _dashboardVM.CurrentView = editView;
            }
        }

        private void ExecuteAddStudentCommand(object obj)
        {
            var addView = new AddStudentView();
            addView.DataContext = new AddStudentVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }

        public void FilterStudents()
        {
            string search = StudentSearchText.ToLower();

            using var context = new AttendanceMonitoringContext();
            var students = context.Students
                .Where(c =>
                    c.StudentId.ToString().Contains(search) ||
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    c.LRN.ToLower().Contains(search) ||
                    c.PhoneNumber.ToLower().Contains(search))
                .ToList();

            StudentList.Clear();
            foreach (var s in students)
            {
                StudentList.Add(s);
            }

        }

    }
}
