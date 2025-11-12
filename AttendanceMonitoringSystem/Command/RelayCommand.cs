using System.Windows.Input;

namespace AttendanceMonitoringSystem.Commands;

public class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private Action<object> _execute { get; set; }
    private Predicate<object> _canExecute { get; set; }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Action<object> execute)
    {
        this._execute = execute;
        this._canExecute = null;
    }

    //Events
    public event EventHandler canExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    public bool CanExecute(object? parameter)
    {
        if (_canExecute != null) return _canExecute(parameter);
        else return true;
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}