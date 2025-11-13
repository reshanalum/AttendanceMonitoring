using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class EditSectionVM
    {
        private readonly DashboardVM _dashboardVM;

        public ObservableCollection<StudentDisplay> StudentList { get; set; } = new();
        public ObservableCollection<Class_Adviser> TeacherList { get; set; } = new ObservableCollection<Class_Adviser>();

        // Section being edited
        public Advisory EditingSection { get; set; }

        // Selected teacher for section
        public Class_Adviser EditingSelectedTeacher { get; set; }

        public EditSectionVM(DashboardVM dashboardVM, Advisory sectionToEdit)
        {
            _dashboardVM = dashboardVM;

            EditingSection = sectionToEdit;
            EditingSelectedTeacher = sectionToEdit.ClassAdviserLink;

            LoadTeachers();
            LoadStudentsInSection(sectionToEdit.AdvisoryId);
        }

        /// <summary>
        /// Load all teachers
        /// </summary>
        private void LoadTeachers()
        {
            using var context = new AttendanceMonitoringContext();
            var teachers = context.Class_Advisers.ToList();

            TeacherList.Clear();
            foreach (var t in teachers)
            {
                TeacherList.Add(t);
            }
        }

        /// <summary>
        /// Load only students currently in the section
        /// </summary>
        private void LoadStudentsInSection(int sectionId)
        {
            using var context = new AttendanceMonitoringContext();

            var studentsInSection = context.Students
                .Where(s => s.AdvisoryList.Any(a => a.AdvisoryId == sectionId))
                .Select(s => new StudentDisplay
                {
                    Student = s,
                    IsSelected = true // mark as selected since they are in this section
                })
                .ToList();

            StudentList.Clear();
            foreach (var s in studentsInSection)
            {
                StudentList.Add(s);
            }
        }

        /// <summary>
        /// Save the changes to the section
        /// </summary>
        public void SaveCommand()
        {
            if (EditingSection == null || EditingSelectedTeacher == null)
            {
                MessageBox.Show("Section or adviser is not set.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();

            // Reload the section including its students
            var section = context.Advisories
                .Where(a => a.AdvisoryId == EditingSection.AdvisoryId)
                .ToList();

            // Students currently selected in the UI
            var selectedStudentIds = StudentList.Where(s => s.IsSelected).Select(s => s.Student.StudentId).ToList();

            // Remove students who are no longer selected
            var studentsToRemove = context.Advisories
                .Where(a => a.SectionName == EditingSection.SectionName &&
                            !selectedStudentIds.Contains(a.StudentId))
                .ToList();

            foreach (var s in studentsToRemove)
            {
                context.Advisories.Remove(s);
            }

            // Add new students who are selected but not yet in the section
            var existingStudentIds = context.Advisories
                .Where(a => a.SectionName == EditingSection.SectionName)
                .Select(a => a.StudentId)
                .ToList();

            var studentsToAdd = selectedStudentIds
                .Where(id => !existingStudentIds.Contains(id))
                .ToList();

            foreach (var studentId in studentsToAdd)
            {
                var newAdvisory = new Advisory
                {
                    SectionName = EditingSection.SectionName,
                    SchoolYear = EditingSection.SchoolYear,
                    ClassAdviserId = EditingSelectedTeacher.ClassAdviserId,
                    StudentId = studentId
                };
                context.Advisories.Add(newAdvisory);
            }

            // Update section adviser if changed
            var advisoriesToUpdate = context.Advisories
                .Where(a => a.SectionName == EditingSection.SectionName)
                .ToList();

            foreach (var a in advisoriesToUpdate)
            {
                a.ClassAdviserId = EditingSelectedTeacher.ClassAdviserId;
            }

            context.SaveChanges();

            MessageBox.Show("Section updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            BackToSectionList();
        }

        /// <summary>
        /// Navigate back to the section list
        /// </summary>
        public void BackToSectionList()
        {
            var sectionView = new SectionListView(_dashboardVM);
            sectionView.DataContext = new SectionListVM(_dashboardVM);
            _dashboardVM.CurrentView = sectionView;
        }
    }

    /// <summary>
    /// Wrapper class to display students with selection
    /// </summary>

}
