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
    public class AssignRFIDVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        private List<Student> _allStudentsCache;

        private Student _editingStudent;
        public Student EditingStudent
        {
            get => _editingStudent;
            set { _editingStudent = value; OnPropertyChanged(); }
        }

        // Change the collection type to our new display class
        private ObservableCollection<StudentDisplayItem> _filteredStudents;
        public ObservableCollection<StudentDisplayItem> FilteredStudents
        {
            get => _filteredStudents;
            set { _filteredStudents = value; OnPropertyChanged(); }
        }

        private bool _isInternalUpdate; // Flag to prevent the filtering loop
        private string _studentSearchText;

        public string StudentSearchText
        {
            get => _studentSearchText;
            set
            {
                _studentSearchText = value;
                OnPropertyChanged();

                // ONLY filter if this wasn't triggered by selecting an item
                // and if the name isn't ReadOnly
                if (!_isInternalUpdate && !IsNameReadOnly)
                {
                    FilterStudents(value);
                }
            }
        }

        // Property to handle the user choosing a name
        private StudentDisplayItem _selectedStudentItem;
        public StudentDisplayItem SelectedStudentItem
        {
            get => _selectedStudentItem;
            set
            {
                _selectedStudentItem = value;
                OnPropertyChanged();

                if (value != null)
                {
                    _isInternalUpdate = true; // Raise the flag

                    EditingStudent.StudentId = value.Student.StudentId;
                    EditingStudent.FirstName = value.Student.FirstName;
                    EditingStudent.LastName = value.Student.LastName;

                    StudentSearchText = value.FullName; // This triggers the setter above

                    _isInternalUpdate = false; // Lower the flag
                }
            }
        }

        private bool _isNameReadOnly;
        public bool IsNameReadOnly
        {
            get => _isNameReadOnly;
            set { _isNameReadOnly = value; OnPropertyChanged(); }
        }



        public AssignRFIDVM(DashboardVM dashboardVM, string scannedRFID)
        {
            _dashboardVM = dashboardVM;
            using var context = new AttendanceMonitoringContext();
            _allStudentsCache = context.Students.ToList();

            InitializeAssignment(scannedRFID);
        }

        private void InitializeAssignment(string rfid)
        {
            var existingOwner = _allStudentsCache.FirstOrDefault(s => s.RFID == rfid);

            if (existingOwner != null)
            {
                EditingStudent = existingOwner;
                StudentSearchText = $"{existingOwner.FirstName} {existingOwner.LastName}";
                IsNameReadOnly = true;
            }
            else
            {
                EditingStudent = new Student { RFID = rfid };
                StudentSearchText = string.Empty;
                IsNameReadOnly = false;
            }
        }

        private void FilterStudents(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FilteredStudents = new ObservableCollection<StudentDisplayItem>();
                return;
            }

            var results = _allStudentsCache
                .Where(s => (s.FirstName + " " + s.LastName).Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .Select(s => new StudentDisplayItem(s)) // Wrap the Student model
                .ToList();

            FilteredStudents = new ObservableCollection<StudentDisplayItem>(results);
        }

        public void SaveCommand()
        {
            // 1. Validation: Ensure a student has been selected/found and there is an RFID to assign
            if (EditingStudent == null || EditingStudent.StudentId == 0)
            {
                MessageBox.Show("Please select a student from the search results to assign the RFID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(EditingStudent.RFID))
            {
                MessageBox.Show("No RFID code detected to assign.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            // 2. Find the existing student in the database to update
            var studentInDb = context.Students.FirstOrDefault(s => s.StudentId == EditingStudent.StudentId);

            if (studentInDb != null)
            {
                // Assign the scanned RFID to the student
                studentInDb.RFID = EditingStudent.RFID;

                context.SaveChanges();

                MessageBox.Show($"RFID successfully assigned to {studentInDb.FirstName} {studentInDb.LastName}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // 3. Return to the previous list
                BackToNotificationList();
            }
            else
            {
                MessageBox.Show("The selected student record could not be found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void BackToNotificationList()
        {

           var notificationView = new NotificationListView(_dashboardVM);
            notificationView.DataContext = new NotificationListVM(_dashboardVM);
            _dashboardVM.CurrentView = notificationView;
        }

    }

    public class StudentDisplayItem
    {
        // Store the actual student object for later use
        public Student Student { get; set; }

        // This is what the ComboBox will display
        public string FullName => $"{Student.FirstName} {Student.LastName}";

        public StudentDisplayItem(Student student)
        {
            Student = student;
        }
    }
}
