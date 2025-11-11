using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<string> EnrollmentStatus { get; set; }
        public EditStudentVM(Student selectedStudent, DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;


            EnrollmentStatus = new List<string>
            {
                "Enrolled",
                "Not Enrolled"
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
                MessageBox.Show("The selected student could not be found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var relationship = context.Relationships.FirstOrDefault(r => r.StudentId == EditingStudent.StudentId);
            if (relationship == null)
            {
                MessageBox.Show("No parent record found for this student.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            EditingParent = context.Parents.FirstOrDefault(p => p.ParentId == relationship.ParentId);
            if (EditingParent == null)
            {
                MessageBox.Show("Parent details could not be loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var contacts = context.Contacts.Where(c => c.ParentId == EditingParent.ParentId).ToList();

            Contact1 = contacts.Count > 0 ? contacts[0] : new Contact { ParentId = EditingParent.ParentId };
            Contact2 = contacts.Count > 1 ? contacts[1] : new Contact { ParentId = EditingParent.ParentId };

            OnPropertyChanged(nameof(EditingStudent));
            OnPropertyChanged(nameof(EditingParent));
            OnPropertyChanged(nameof(Contact1));
            OnPropertyChanged(nameof(Contact2));
        }


        public void SaveEditedStudent()
        {
            if (EditingStudent == null)
            {
                MessageBox.Show("No student selected for editing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            var studentInDb = context.Students.FirstOrDefault(s => s.StudentId == EditingStudent.StudentId);
            if (studentInDb != null)
            {
                studentInDb.FirstName = EditingStudent.FirstName;
                studentInDb.LastName = EditingStudent.LastName;
                studentInDb.LRN = EditingStudent.LRN;
                studentInDb.PhoneNumber = EditingStudent.PhoneNumber;
                studentInDb.EnrollmentStatus = EditingStudent.EnrollmentStatus;
            }

            var parentInDb = context.Parents.FirstOrDefault(p => p.ParentId == EditingParent.ParentId);
            if (parentInDb != null)
            {
                parentInDb.FirstName = EditingParent.FirstName;
                parentInDb.LastName = EditingParent.LastName;
                parentInDb.Email = EditingParent.Email;
            }

            if (Contact1 != null && !string.IsNullOrWhiteSpace(Contact1.PhoneNumber))
            {
                var contact1InDb = context.Contacts.FirstOrDefault(c => c.ContactId == Contact1.ContactId);
                if (contact1InDb != null)
                {
                    contact1InDb.PhoneNumber = Contact1.PhoneNumber;
                    contact1InDb.Network = Contact1.Network ?? "";
                }
                else
                {
                    Contact1.ParentId = EditingParent.ParentId;
                    context.Contacts.Add(Contact1);
                }
            }

            if (Contact2 != null && !string.IsNullOrWhiteSpace(Contact2.PhoneNumber))
            {
                var contact2InDb = context.Contacts.FirstOrDefault(c => c.ContactId == Contact2.ContactId);
                if (contact2InDb != null)
                {
                    contact2InDb.PhoneNumber = Contact2.PhoneNumber;
                    contact2InDb.Network = Contact2.Network ?? "";
                }
                else
                {
                    Contact2.ParentId = EditingParent.ParentId;
                    context.Contacts.Add(Contact2);
                }
            }

            context.SaveChanges();
            MessageBox.Show("Student and parent details updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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


