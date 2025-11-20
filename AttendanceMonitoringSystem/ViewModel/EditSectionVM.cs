using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringSystem.View;
using Microsoft.EntityFrameworkCore;
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

        public Advisory EditingSection { get; set; }
        public string EditingSectionName { get; set; }
        public Class_Adviser EditingSelectedTeacher { get; set; }

        public EditSectionVM(DashboardVM dashboardVM, Advisory sectionToEdit)
        {
            _dashboardVM = dashboardVM;

            EditingSection = sectionToEdit;
            EditingSelectedTeacher = sectionToEdit.ClassAdviserLink;
            EditingSectionName = sectionToEdit.SectionName;

            LoadTeachers();
            LoadStudentsInSection(sectionToEdit);
        }
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
        private void LoadStudentsInSection(Advisory section)
        {
            using var context = new AttendanceMonitoringContext();

            var studentsInSectionIds = context.Advisories
                .Where(a => a.SectionName == section.SectionName)
                .Select(a => a.StudentId)
                .ToList();
            var allRelevantStudents = context.Students
                .Where(s => studentsInSectionIds.Contains(s.StudentId)
                            || !s.AdvisoryList.Any())
                .ToList();

            StudentList.Clear();
            foreach (var student in allRelevantStudents)
            {
                StudentList.Add(new StudentDisplay
                {
                    Student = student,
                    IsSelected = studentsInSectionIds.Contains(student.StudentId) 
                });
            }
        }
        public void SaveCommand()
        {
            if (EditingSection == null || EditingSelectedTeacher == null)
            {
                MessageBox.Show("Section or adviser is not set.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();


            var section = context.Advisories
                .Where(a => a.AdvisoryId == EditingSection.AdvisoryId)
                .ToList();


            var selectedStudentIds = StudentList.Where(s => s.IsSelected).Select(s => s.Student.StudentId).ToList();


            var studentsToRemove = context.Advisories
                .Where(a => a.SectionName == EditingSection.SectionName &&
                            !selectedStudentIds.Contains(a.StudentId))
                .ToList();

            foreach (var s in studentsToRemove)
            {
                context.Advisories.Remove(s);
            }

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
        public void BackToSectionList()
        {
            var sectionView = new SectionListView(_dashboardVM);
            sectionView.DataContext = new SectionListVM(_dashboardVM);
            _dashboardVM.CurrentView = sectionView;
        }
    }

}
