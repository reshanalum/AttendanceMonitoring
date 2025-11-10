using AttendanceMonitoringSystem.Commands;
using AttendanceMonitoringSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AttendanceMonitoring.Models;

namespace AttendanceMonitoringSystem.ViewModel
{
    public class StudentListVM: NotifyPropertyChanged
    {
        private readonly DashboardVM _dashboardVM;
        public ICommand ShowEditStudentCommand { get; set; }
        public ICommand ShowAddStudentCommand { get; set; }

        private Employee selectedEmployee;

        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        private string employeeSearchText;

        public string EmployeeSearchText
        {
            get => employeeSearchText;
            set
            {
                employeeSearchText = value;
                FilterEmployees();
            }
        }

        private void LoadEmployees()
        {
            using var context = new GardenGloryContext();
            var employees = context.Employees.ToList();

            EmployeesList.Clear();
            foreach (var emp in employees)
            {
                EmployeesList.Add(emp);
            }
        }

        public StudentListVM(DashboardVM _dashboardVM)
        {

        }

        public void DeleteEmployee(object obj)
        {

            if (obj is not Employee employeeToDelete)
            {
                MessageBox.Show("No employee selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete employee: {employeeToDelete.FirstName} {employeeToDelete.LastName}?",
                "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var context = new GardenGloryContext();
            var employeeInDb = context.Employees.FirstOrDefault(c => c.EmployeeId == employeeToDelete.EmployeeId);

            if (employeeInDb == null)
            {
                MessageBox.Show("The selected employee does not exist in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Employees.Remove(employeeInDb);
            context.SaveChanges();
            EmployeesList.Remove(employeeToDelete);
            SelectedEmployee = null;
        }



        private void ExecuteEditEmployeeCommand(object obj)
        {
            if (obj is Employee employee)
            {
                SelectedEmployee = employee;
                var editView = new EditEmployee();
                editView.DataContext = new EditEmployeeVM(employee, _mainVM);
                _mainVM.CurrentView = editView;
            }
        }

        private void ExecuteAddEmployeeCommand(object obj)
        {
            var addView = new AddEmployee();
            addView.DataContext = new AddEmployeeVM(_mainVM);
            _mainVM.CurrentView = addView;
        }

        public void FilterEmployees()
        {
            string search = EmployeeSearchText.ToLower();

            using var context = new GardenGloryContext();
            var employees = context.Employees
                .Where(c =>
                    c.EmployeeId.ToString().Contains(search) ||
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    c.CellPhone.ToLower().Contains(search) ||
                    c.ExperienceLevel.ToLower().Contains(search))
                .ToList();

            EmployeesList.Clear();
            foreach (var emp in employees)
            {
                EmployeesList.Add(emp);
            }

        }

    }
}
