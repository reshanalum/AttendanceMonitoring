using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class EditStudentVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        public Student EditingStudent { get; set; }
        public Parent EditingParent { get; set; }
        public Contact Contact1 { get; set; }
        public Contact Contact2 { get; set; }

        // ComboBox choices
        public List<string> EnrollmentStatus { get; set; }

        private string _selectedEnrollmentStatus;
        public string SelectedEnrollmentStatus
        {
            get => _selectedEnrollmentStatus;
            set
            {
                if (_selectedEnrollmentStatus != value)
                {
                    _selectedEnrollmentStatus = value;
                    OnPropertyChanged(nameof(SelectedEnrollmentStatus));

                    // update student value
                    if (EditingStudent != null)
                        EditingStudent.EnrollmentStatus = value;
                }
            }
        }

        public EditStudentVM(Student selectedStudent, DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            // ComboBox items
            EnrollmentStatus = new List<string>
            {
                "ENROLLED",
                "NOT ENROLLED"
            };

            Contact1 = new Contact();
            Contact2 = new Contact();

            LoadStudentAndParent(selectedStudent);
        }

        private void LoadStudentAndParent(Student student)
        {
            using var context = new AttendanceMonitoringContext();

            EditingStudent = context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (EditingStudent == null)
            {
                MessageBox.Show("Selected student not found.");
                return;
            }

            var relationship = context.Relationships.FirstOrDefault(r => r.StudentId == EditingStudent.StudentId);
            if (relationship == null)
            {
                MessageBox.Show("No parent assigned.");
                return;
            }

            EditingParent = context.Parents.FirstOrDefault(p => p.ParentId == relationship.ParentId);

            var contacts = context.Contacts.Where(c => c.ParentId == EditingParent.ParentId).ToList();
            Contact1 = contacts.Count > 0 ? contacts[0] : new Contact { ParentId = EditingParent.ParentId };
            Contact2 = contacts.Count > 1 ? contacts[1] : new Contact { ParentId = EditingParent.ParentId };

            // IMPORTANT: Set the ComboBox selected item
            SelectedEnrollmentStatus = EditingStudent.EnrollmentStatus?.Trim();
            OnPropertyChanged(nameof(SelectedEnrollmentStatus));

            OnPropertyChanged(nameof(EditingStudent));
            OnPropertyChanged(nameof(EditingParent));
            OnPropertyChanged(nameof(Contact1));
            OnPropertyChanged(nameof(Contact2));
        }

        public void SaveEditedStudent()
        {
            using var context = new AttendanceMonitoringContext();

            var studentInDb = context.Students.FirstOrDefault(s => s.StudentId == EditingStudent.StudentId);
            if (studentInDb != null)
            {
                studentInDb.FirstName = EditingStudent.FirstName;
                studentInDb.LastName = EditingStudent.LastName;
                studentInDb.LRN = EditingStudent.LRN;
                studentInDb.EnrollmentStatus = EditingStudent.EnrollmentStatus;
            }

            var parentInDb = context.Parents.FirstOrDefault(p => p.ParentId == EditingParent.ParentId);
            if (parentInDb != null)
            {
                parentInDb.FirstName = EditingParent.FirstName;
                parentInDb.LastName = EditingParent.LastName;
            }

            if (!string.IsNullOrWhiteSpace(Contact1.PhoneNumber))
            {
                var c1 = context.Contacts.FirstOrDefault(c => c.ContactId == Contact1.ContactId);
                if (c1 != null)
                    c1.PhoneNumber = Contact1.PhoneNumber;
                else
                    context.Contacts.Add(Contact1);
            }

            if (!string.IsNullOrWhiteSpace(Contact2.PhoneNumber))
            {
                var c2 = context.Contacts.FirstOrDefault(c => c.ContactId == Contact2.ContactId);
                if (c2 != null)
                    c2.PhoneNumber = Contact2.PhoneNumber;
                else
                    context.Contacts.Add(Contact2);
            }

            context.SaveChanges();

            MessageBox.Show("Saved successfully!", "Success");
            BackToStudentList();
        }

        public void BackToStudentList()
        {
            var studentView = new StudentListView(_dashboardVM);
            studentView.DataContext = new StudentListVM(_dashboardVM);
            _dashboardVM.CurrentView = studentView;
        }
    }
}
