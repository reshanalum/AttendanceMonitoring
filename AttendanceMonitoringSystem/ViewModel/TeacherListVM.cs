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
    public class TeacherListVM : NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;

        private string _searchText;

        public ObservableCollection<TeacherDisplay> TeacherList { get; set; }
            = new ObservableCollection<TeacherDisplay>();

        public ICommand ShowEditTeacherCommand { get; set; }
        public ICommand ShowAddTeacherCommand { get; set; }

        public ICommand DeleteTeacherCommand { get; set; }

        public ICommand ShowTeacherInformationCommand { get; set; }

        private TeacherDisplay _selectedTeacher;
        public TeacherDisplay SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged();
            }
        }

        public TeacherListVM(DashboardVM dashboardVM)
        {
            _dashboardVM = dashboardVM;

            ShowEditTeacherCommand = new RelayCommand(ExecuteEditTeacherCommand);
            ShowAddTeacherCommand = new RelayCommand(ExecuteAddTeacherCommand);

            DeleteTeacherCommand = new RelayCommand(DeleteTeacher);
            ShowTeacherInformationCommand = new RelayCommand(ExecuteShowTeacherInformation);

            LoadTeachers();
        }

        // -------------------------------------------------------------
        // LOAD TEACHERS + THEIR ADVISORY CLASS
        // -------------------------------------------------------------
        private void LoadTeachers()
        {
            using var context = new AttendanceMonitoringContext();

            var advisers = context.Class_Advisers
                .Select(a => new TeacherDisplay
                {
                    ClassAdviserId = a.ClassAdviserId,
                    FirstName = a.FirstName,
                    LastName = a.LastName,

                    SectionName = a.AdvisoryList
                        .Select(s => s.SectionName)
                        .FirstOrDefault() ?? "None"
                })
                .ToList();

            TeacherList.Clear();
            foreach (var t in advisers)
                TeacherList.Add(t);
        }

        // -------------------------------------------------------------
        // SHOW INFORMATION
        // -------------------------------------------------------------
        private void ExecuteShowTeacherInformation(object obj)
        {
            if (SelectedTeacher == null)
            {
                MessageBox.Show("Please select a teacher first.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var adviser = context.Class_Advisers
                .FirstOrDefault(t => t.ClassAdviserId == SelectedTeacher.ClassAdviserId);

            if (adviser == null)
            {
                MessageBox.Show("Teacher not found in database.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var view = new TeacherInformationView();
            //view.DataContext = new TeacherInformationVM(adviser, _dashboardVM);
            _dashboardVM.CurrentView = view;
        }

        // -------------------------------------------------------------
        // DELETE TEACHER
        // -------------------------------------------------------------
        public void DeleteTeacher(object obj)
        {
            if (SelectedTeacher == null)
            {
                MessageBox.Show("No teacher selected.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete: {SelectedTeacher.FirstName} {SelectedTeacher.LastName}?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var context = new AttendanceMonitoringContext();

            var teacherInDb = context.Class_Advisers
                .FirstOrDefault(c => c.ClassAdviserId == SelectedTeacher.ClassAdviserId);

            if (teacherInDb == null)
            {
                MessageBox.Show("Teacher does not exist in database.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Class_Advisers.Remove(teacherInDb);
            context.SaveChanges();

            TeacherList.Remove(SelectedTeacher);
            SelectedTeacher = null;
        }

        // -------------------------------------------------------------
        // EDIT TEACHER
        // -------------------------------------------------------------
        private void ExecuteEditTeacherCommand(object obj)
        {
            if (SelectedTeacher == null)
            {
                MessageBox.Show("Please select a teacher to edit.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new AttendanceMonitoringContext();
            var adviser = context.Class_Advisers
                .FirstOrDefault(t => t.ClassAdviserId == SelectedTeacher.ClassAdviserId);

            if (adviser == null)
            {
                MessageBox.Show("Teacher not found in database.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editView = new EditTeacherView();
            //editView.DataContext = new EditTeacherVM(adviser, _dashboardVM);
            _dashboardVM.CurrentView = editView;
        }

        // -------------------------------------------------------------
        // ADD TEACHER
        // -------------------------------------------------------------
        private void ExecuteAddTeacherCommand(object obj)
        {
            var addView = new AddTeacherView();
            //addView.DataContext = new AddTeacherVM(_dashboardVM);
            _dashboardVM.CurrentView = addView;
        }

        // -------------------------------------------------------------
        // SEARCH FILTER
        // -------------------------------------------------------------
        public string TeacherSearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                FilterTeachers();
                OnPropertyChanged();
            }
        }

        private void FilterTeachers()
        {
            LoadTeachers(); // reset list

            if (string.IsNullOrWhiteSpace(TeacherSearchText))
                return;

            var search = TeacherSearchText.ToLower();

            var filtered = TeacherList
                .Where(t =>
                       t.FirstName.ToLower().Contains(search)
                    || t.LastName.ToLower().Contains(search)
                    || t.ClassAdviserId.ToString().Contains(search)
                    || t.SectionName.ToLower().Contains(search))
                .ToList();

            TeacherList.Clear();
            foreach (var t in filtered)
                TeacherList.Add(t);
        }
    }

    // -----------------------------
    // DISPLAY MODEL FOR VIEW
    // -----------------------------
    public class TeacherDisplay
    {
        public int ClassAdviserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SectionName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
